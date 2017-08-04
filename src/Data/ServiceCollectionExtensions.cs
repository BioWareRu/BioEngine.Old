using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Search;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Data
{
    public static class ServiceCollectionExtensions
    {
        public static void AddBioEngineData(this IServiceCollection services, IConfigurationRoot configuration)
        {
            services.AddScoped(typeof(ISearchProvider<>), typeof(ElasticSearchProvider<>));
            services.AddScoped<ParentEntityProvider>();
            var dbConfig = new MySqlDBConfiguration(configuration);
            services.AddDbContext<BWContext>(builder => dbConfig.Configure(builder));
            services.AddMediatR();

            services.RegisterMediatRHandlers();
        }

        private static void RegisterMediatRHandlers(this IServiceCollection services)
        {
            var assembly = typeof(QueryBase<>).GetTypeInfo().Assembly;
            var openInterfaces = new[]
            {
                typeof(IAsyncRequestHandler<,>),
                typeof(IAsyncNotificationHandler<>)
            };

            foreach (var openInterface in openInterfaces)
            {
                var concretions = new List<Type>();
                var interfaces = new List<Type>();

                foreach (var typeInfo in assembly.DefinedTypes)
                {
                    var type = typeInfo.AsType();
                    IEnumerable<Type> interfaceTypes = type.FindInterfacesThatClose(openInterface).ToArray();
                    if (!interfaceTypes.Any()) continue;

                    if (type.IsConcrete())
                    {
                        concretions.Add(type);
                    }

                    foreach (var interfaceType in interfaceTypes)
                    {
                        interfaces.Fill(interfaceType);
                    }
                }

                foreach (var @interface in interfaces)
                {
                    var exactMatches = concretions.Where(t => t.CanBeCastTo(@interface)).ToArray();

                    foreach (var exactMatch in exactMatches)
                    {
                        services.AddTransient(@interface, exactMatch);
                    }
                }
            }
        }
    }
}