using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using Autofac.Features.Variance;
using BioEngine.Common.Base;
using BioEngine.Common.DB;
using BioEngine.Common.Search;
using BioEngine.Data.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace BioEngine.Data
{
    public static class ContainerBuilderExtensions
    {
        public static void AddBioEngineData(this ContainerBuilder containerBuilder, IConfigurationRoot configuration)
        {
            containerBuilder.RegisterGeneric(typeof(ElasticSearchProvider<>)).As(typeof(ISearchProvider<>))
                .InstancePerLifetimeScope();
            containerBuilder.RegisterType<ParentEntityProvider>().InstancePerLifetimeScope();
            var dbConfig = new MySqlDBConfiguration(configuration);
            containerBuilder.AddDbContext<BWContext>(builder => dbConfig.Configure(builder));

            containerBuilder
                .RegisterSource(new ContravariantRegistrationSource());

            containerBuilder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            containerBuilder
                .Register<SingleInstanceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t =>
                    {
                        object o;
                        return c.TryResolve(t, out o) ? o : null;
                    };
                })
                .InstancePerLifetimeScope();

            containerBuilder
                .Register<MultiInstanceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => (IEnumerable<object>) c.Resolve(typeof(IEnumerable<>).MakeGenericType(t));
                })
                .InstancePerLifetimeScope();

            containerBuilder.RegisterAssemblyTypes(typeof(HandlerBase).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("Handler")).AsImplementedInterfaces();
        }

        public static ContainerBuilder AddDbContext<TContext>(
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
        }
    }
}