using System.Reflection;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Models;
using BioEngine.Common.Search;
using BioEngine.Data.Articles.Handlers;
using BioEngine.Data.Articles.Requests;
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
            services.AddMediatR(typeof(RequestBase<>).GetTypeInfo().Assembly);
            
            /*services
                .AddTransient<IAsyncRequestHandler<ArticleCategoryProcessRequest, ArticleCat>,
                    ArticlesCategoryProcessHandler>();*/
            
            
        }
    }
}