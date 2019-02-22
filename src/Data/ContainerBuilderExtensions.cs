using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Variance;
using AutoMapper;
using BioEngine.Data.Base;
using BioEngine.Data.DB;
using BioEngine.Data.Core;
using BioEngine.Data.Search.Handlers;
using BioEngine.Search;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Data
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddBioEngineData(this IServiceCollection services,
            IConfiguration configuration)
        {
            var dbConfig = new MySqlDBConfiguration(configuration);
            services.AddDbContextPool<BWContext>(connectionBuilder => dbConfig.Configure(connectionBuilder));

            services.AddBioEngineSearch(configuration);

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.AddBioEngineData();
            return builder;
        }

        public static void AddBioEngineData(this ContainerBuilder containerBuilder)
        {
            containerBuilder.AddBioEngineSearch();
            containerBuilder.RegisterType<ParentEntityProvider>().InstancePerLifetimeScope();

            containerBuilder
                .RegisterAssemblyTypes(typeof(BioRepository).GetTypeInfo().Assembly)
                .AsClosedTypesOf(typeof(BaseBioRepository<,>))
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<BioRepository>().InstancePerLifetimeScope();

            containerBuilder
                .RegisterSource(new ContravariantRegistrationSource());

            containerBuilder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            containerBuilder.Register<ServiceFactory>(context =>
            {
                var c = context.Resolve<IComponentContext>();
                return t => c.Resolve(t);
            });

            containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            containerBuilder.RegisterGeneric(typeof(HandlerContext<>)).InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Validator")).AsImplementedInterfaces().InstancePerDependency();

            containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("MapperProfile")).As<Profile>();

            containerBuilder.RegisterGeneric(typeof(SearchEntitiesHandler<>)).AsImplementedInterfaces()
                .InstancePerDependency();
            containerBuilder.RegisterGeneric(typeof(CountEntitiesHandler<>)).AsImplementedInterfaces()
                .InstancePerDependency();
            containerBuilder.RegisterGeneric(typeof(IndexEntitiesHandler<>)).AsImplementedInterfaces()
                .InstancePerDependency();
            containerBuilder.RegisterGeneric(typeof(IndexEntityHandler<>)).AsImplementedInterfaces()
                .InstancePerDependency();
            containerBuilder.RegisterGeneric(typeof(DeleteEntityFromIndexHandler<>)).AsImplementedInterfaces()
                .InstancePerDependency();

            containerBuilder.Register(context => new MapperConfiguration(cfg =>
            {
                foreach (var profile in context.Resolve<IEnumerable<Profile>>())
                {
                    cfg.AddProfile(profile);
                }
            })).AsSelf().SingleInstance();

            containerBuilder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>().InstancePerLifetimeScope();
        }
    }
}