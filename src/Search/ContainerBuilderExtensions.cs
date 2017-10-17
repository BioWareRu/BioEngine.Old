using System.Reflection;
using Autofac;
using AutoMapper;
using BioEngine.Search.ElasticSearch;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Search
{
    public static class ContainerBuilderExtensions
    {
        public static void AddBioEngineSearch(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<ElasticSearchProviderConfig>(o => { o.Url = configuration["BE_ES_URL"]; });
        }

        public static void AddBioEngineSearch(this ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterAssemblyTypes(typeof(SearchMapperProfile).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("MapperProfile")).As<Profile>();

            containerBuilder.RegisterAssemblyTypes(typeof(ElasticSearchProvider<,>).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("SearchProvider")).AsImplementedInterfaces();
        }
    }
}