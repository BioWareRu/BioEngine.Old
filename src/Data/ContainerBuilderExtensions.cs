using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Features.Variance;
using AutoMapper;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Search;
using BioEngine.Data.Core;
using MediatR;
using MediatR.Pipeline;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BioEngine.Data
{
    public static class ContainerBuilderExtensions
    {
        public static ContainerBuilder AddBioEngineData(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            var dbConfig = new MySqlDBConfiguration(configuration);
            services.AddDbContext<BWContext>(connectionBuilder => dbConfig.Configure(connectionBuilder));

            var builder = new ContainerBuilder();
            builder.Populate(services);
            builder.AddBioEngineData(configuration);
            return builder;
        }

        public static void AddBioEngineData(this ContainerBuilder containerBuilder, IConfigurationRoot configuration)
        {
            containerBuilder.RegisterGeneric(typeof(ElasticSearchProvider<>)).As(typeof(ISearchProvider<>))
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ParentEntityProvider>().InstancePerLifetimeScope();

            containerBuilder
                .RegisterSource(new ContravariantRegistrationSource());

            containerBuilder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            var mediatrOpenTypes = new[]
            {
                typeof(IRequestHandler<,>),
                typeof(IAsyncRequestHandler<,>),
                typeof(ICancellableAsyncRequestHandler<,>),
                typeof(INotificationHandler<>),
                typeof(IAsyncNotificationHandler<>),
                typeof(ICancellableAsyncNotificationHandler<>)
            };
            
            foreach (var mediatrOpenType in mediatrOpenTypes)
            {
                containerBuilder
                    .RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                    .AsClosedTypesOf(mediatrOpenType)
                    .AsImplementedInterfaces();
            }
            
            containerBuilder.RegisterGeneric(typeof(RequestPostProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
            containerBuilder.RegisterGeneric(typeof(RequestPreProcessorBehavior<,>)).As(typeof(IPipelineBehavior<,>));

            containerBuilder.Register<SingleInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t =>
                {
                    object o;
                    return c.TryResolve(t, out o) ? o : null;
                };
            });

            containerBuilder.Register<MultiInstanceFactory>(ctx =>
            {
                var c = ctx.Resolve<IComponentContext>();
                return t => (IEnumerable<object>)c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
            });
            
            /*containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces().InstancePerDependency();
            containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Validator")).AsImplementedInterfaces().InstancePerDependency();*/

            containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("MapperProfile")).As<Profile>();

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

        /*public static ContainerBuilder AddDbContext<TContext>(
            this ContainerBuilder containerBuilder,
            Action<DbContextOptionsBuilder> optionsAction = null)
            where TContext : DbContext
            => AddDbContext<TContext>(containerBuilder, optionsAction == null
                ? (Action<IComponentContext, DbContextOptionsBuilder>) null
                : (p, b) => optionsAction.Invoke(b));


        public static ContainerBuilder AddDbContext<TContext>(
            this ContainerBuilder containerBuilder,
            Action<IComponentContext, DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            if (optionsAction != null)
            {
                CheckContextConstructors<TContext>();
            }

            AddCoreServices<TContext>(containerBuilder, optionsAction);

            containerBuilder.RegisterType<TContext>().AsSelf().InstancePerLifetimeScope();

            return containerBuilder;
        }

        private static void AddCoreServices<TContext>(
            ContainerBuilder serviceCollection,
            Action<IComponentContext, DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            serviceCollection.Register(p => DbContextOptionsFactory<TContext>(p, optionsAction))
                .As<DbContextOptions<TContext>>().InstancePerLifetimeScope();


            serviceCollection.Register(p => p.Resolve<DbContextOptions<TContext>>()).As<DbContextOptions>()
                .InstancePerLifetimeScope();
        }

        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
            IComponentContext applicationServiceProvider,
            Action<IComponentContext, DbContextOptionsBuilder> optionsAction)
            where TContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>(new Dictionary<Type, IDbContextOptionsExtension>()));

            optionsAction?.Invoke(applicationServiceProvider, builder);

            return builder.Options;
        }

        private static void CheckContextConstructors<TContext>() where TContext : DbContext
        {
            var declaredConstructors = typeof(TContext).GetTypeInfo().DeclaredConstructors.ToList();
            if (declaredConstructors.Count == 1
                && declaredConstructors[0].GetParameters().Length == 0)
            {
                throw new ArgumentException("Error while check context constructors");
            }
        }*/
    }
}