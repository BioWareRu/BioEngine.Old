using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace BioEngine.Data.Core
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> ApplyPaging<T>(this IQueryable<T> query, int offset, int limit)
        {
            return query.Skip(offset).Take(limit);
        }

        private static List<SortQuery> GetSortParameters<T>(string orderBy)
        {
            var sortParameters = new List<SortQuery>();
            if (!string.IsNullOrEmpty(orderBy))
            {
                orderBy.Split(',').ToList().ForEach(p =>
                {
                    var direction = SortDirection.Ascending;
                    if (p[0] == '-')
                    {
                        direction = SortDirection.Descending;
                        p = p.Substring(1);
                    }

                    var attribute = GetByName(typeof(T), p);
                    if (attribute != null)
                    {
                        sortParameters.Add(new SortQuery(attribute.Name, direction));
                    }
                });
            }
            return sortParameters;
        }

        public static Func<IQueryable<T>, IQueryable<T>> GetOrderByFunc<T>(string orderBy)
        {
            var sortParameters = GetSortParameters<T>(orderBy);
            if (sortParameters.Any())
            {
                return query =>
                    sortParameters.Aggregate(query, (current, sortParameter) => current.Sort(sortParameter));
            }
            return null;
        }

        private static IOrderedQueryable<TSource> Sort<TSource>(this IQueryable<TSource> source, SortQuery sortQuery)
        {
            return sortQuery.SortDirection == SortDirection.Descending
                ? source.OrderByDescending(sortQuery.Name)
                : source.OrderBy(sortQuery.Name);
        }

        private static IOrderedQueryable<TSource> OrderBy<TSource>(this IQueryable<TSource> source, string propertyName)
        {
            return CallGenericOrderMethod(source, propertyName, "OrderBy");
        }

        private static IOrderedQueryable<TSource> OrderByDescending<TSource>(this IQueryable<TSource> source,
            string propertyName)
        {
            return CallGenericOrderMethod(source, propertyName, "OrderByDescending");
        }

        private static IOrderedQueryable<TSource> CallGenericOrderMethod<TSource>(IQueryable<TSource> source,
            string propertyName, string method)
        {
            // {x}
            var parameter = Expression.Parameter(typeof(TSource), "x");
            // {x.propertyName}
            var property = Expression.Property(parameter, propertyName);
            // {x=>x.propertyName}
            var lambda = Expression.Lambda(property, parameter);

            // REFLECTION: source.OrderBy(x => x.Property)
            var orderByMethod = typeof(Queryable).GetTypeInfo().GetMethods()
                .First(x => x.Name == method && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(TSource), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] {source, lambda});

            return (IOrderedQueryable<TSource>) result;
        }

        private static readonly ConcurrentDictionary<Type, ModelStructure> Models = new ConcurrentDictionary<Type, ModelStructure>();

        private static PropertyDescription GetByName(Type cls, string attrName)
        {
            var modelStructure = GetModelStructure(cls);
            var property = modelStructure.Properties
                .FirstOrDefault(propertyInfo => propertyInfo.Name == attrName ||
                                                propertyInfo.Name == attrName || propertyInfo.Column == attrName);
            return property;
        }

        private static ModelStructure GetModelStructure(Type cls)
        {
            if (!Models.ContainsKey(cls))
            {
                Models.TryAdd(cls, ParseClass(cls));
            }
            return Models[cls];
        }

        private static ModelStructure ParseClass(Type cls)
        {
            var modelStructure = new ModelStructure();
            foreach (var propertyInfo in cls.GetTypeInfo().GetProperties())
            {
                modelStructure.Properties.Add(PropertyDescription.Parse(propertyInfo));
            }
            return modelStructure;
        }
    }

    internal class ModelStructure
    {
        public List<PropertyDescription> Properties { get; } = new List<PropertyDescription>();
    }

    internal class PropertyDescription
    {
        public string Name { get; }
        public string Column { get; }

        private PropertyDescription(string name, string column)
        {
            Name = name;
            Column = column;
        }

        public static PropertyDescription Parse(PropertyInfo property)
        {
            string column;
            var columnAttr = property.GetCustomAttribute<ColumnAttribute>();
            if (columnAttr != null)
            {
                column = columnAttr.Name;
            }
            else
            {
                column = string
                    .Concat(property.Name.Select(
                        (x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString()))
                    .ToLower();
            }
            return new PropertyDescription(property.Name, column);
        }
    }

    public enum SortDirection
    {
        Ascending = 1,
        Descending = 2
    }

    internal struct SortQuery
    {
        public readonly string Name;
        public readonly SortDirection SortDirection;

        public SortQuery(string name, SortDirection sortDirection)
        {
            Name = name;
            SortDirection = sortDirection;
        }
    }
}