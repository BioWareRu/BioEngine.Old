using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BioEngine.Data
{
    internal static class TypeExtensions
    {
        internal static void Fill<T>(this ICollection<T> list, T value)
        {
            if (list.Contains(value)) return;
            list.Add(value);
        }
        
        internal static bool CanBeCastTo(this Type pluggedType, Type pluginType)
        {
            if (pluggedType == null) return false;

            if (pluggedType == pluginType) return true;

            return pluginType.GetTypeInfo().IsAssignableFrom(pluggedType.GetTypeInfo());
        }
        
        internal static IEnumerable<Type> FindInterfacesThatClose(this Type pluggedType, Type templateType)
        {
            if (!pluggedType.IsConcrete()) yield break;

            if (templateType.GetTypeInfo().IsInterface)
            {
                var implementedInterfaces = pluggedType.GetTypeInfo().ImplementedInterfaces;
                foreach (
                    var interfaceType in
                    implementedInterfaces
                        .Where(type => type.GetTypeInfo().IsGenericType && type.GetGenericTypeDefinition() == templateType))
                {
                    yield return interfaceType;
                }
            }
        }
        
        internal static bool IsConcrete(this Type type)
        {
            return !type.GetTypeInfo().IsAbstract && !type.GetTypeInfo().IsInterface;
        }
    }
}