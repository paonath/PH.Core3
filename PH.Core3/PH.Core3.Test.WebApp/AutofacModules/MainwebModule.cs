using System;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using Autofac;
using Autofac.Core;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using PH.Core3.AspNetCoreApi.Services.Components;
using PH.Core3.Common;
using PH.Core3.Common.Identifiers;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.Common.Services.Path;
using PH.Core3.EntityFramework;
using PH.Core3.Test.WebApp.HostedService;
using PH.Core3.Test.WebApp.Services;
using PH.Core3.TestContext;
using PH.UowEntityFramework.UnitOfWork;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace PH.Core3.Test.WebApp.AutofacModules
{
    /// <summary>
    /// Main autofac module
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class MainwebModule : Autofac.Module
    {
        private string _connectionString;

        public MainwebModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>Override to add registrations to the container.</summary>
        /// <remarks>
        /// Note that the ContainerBuilder parameter is unique to this module.
        /// </remarks>
        /// <param name="builder">The builder through which components can be
        /// registered.</param>
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<MyContext>()
                   /*
                   .OnPreparing(e => e.Parameters = e.Parameters.Union(
                                                                       new[]
                                                                       {
                                                                           new NamedParameter("babelStringLocalizer",
                                                                                              e.Context
                                                                                               .Resolve<BabelStringLocalizer>()
                                                                                             )

                                                                       }))
                                                                       */
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.Register(c =>
                   {
                       var wPAth = c.ResolveOptional<IHostingEnvironment>()?.WebRootPath ?? "C:\\temp";
                       return new
                           PH.Core3.Common.Services.Components.Path.WebPathTranslator(wPAth);
                   })
                   .AsSelf()
                   .AsImplementedInterfaces()
                   .As<IWebPathTranslator>()
                   .InstancePerLifetimeScope();

            builder.Register(c =>
                   {
                       //ClaimsPrincipal principal = null;
                       //if (c.TryResolve(typeof(IPrincipal), out var principal2))
                       //    principal = principal2 as ClaimsPrincipal;

                       var principal = c.Resolve<IPrincipal>() as ClaimsPrincipal;

                       string iid = $"{Guid.NewGuid():N}";
                       MappedDiagnosticsLogicalContext.Set("IID", iid);

                       return new PH.Core3.Common.Identifiers.ClaimsPrincipalIdentifier(iid, principal);
                   })
                   .AsSelf()
                   .As<IIdentifier>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            builder.Register(c =>
                   {
                       var ctx = c.Resolve<MyContext>();
                       var id  = c.Resolve<ClaimsPrincipalIdentifier>();
                       ctx.Identifier = id.Uid;
                       ctx.Author     = id.Name;
                       //ctx.TenantName = "ABC";

                       //return new EntityFrameworkUnitOfWork(ctx, c.Resolve<ILogger<EntityFrameworkUnitOfWork>>());

                       ctx.UowLogger  = c.Resolve<ILogger<IUnitOfWork>>();
                       ctx.Initialize();

                       return ctx;
                   })
                   //.AsSelf()
                   .As<IUnitOfWork>()
                   .AsImplementedInterfaces()
                   .InstancePerLifetimeScope();

            //builder.Register(c => )

            // If you want to set up a controller for, say, property injection
            // you can override the controller registration after populating services.
            //builder.RegisterType<MyController>().PropertiesAutowired();


            builder.RegisterType<ViewRenderService>().AsSelf().AsImplementedInterfaces().InstancePerLifetimeScope();

            builder.RegisterType<MailSenderService>().AsSelf().AsImplementedInterfaces().SingleInstance();

            var v1Svcs = typeof(DataService).Assembly.GetTypes().Where(t => t.IsAbstract == false).ToArray();

            builder.Register(c => new TransientCrudSettings(c.Resolve<IIdentifier>(),
                                                            c.Resolve<ILogger<TransientCrudSettings>>(), true, true,
                                                            true, true)).AsSelf().AsImplementedInterfaces()
                   .InstancePerLifetimeScope();


            foreach (var serviceType in v1Svcs)
            {
                builder.RegisterType(serviceType)
                       .AsSelf()
                       .AsImplementedInterfaces()
                       .InstancePerLifetimeScope()
                       .OnActivated(e =>
                       {
                           //var svc = (Ecube.Ssgu.Api.Common.Services.IService) e.Instance;
                           //svc.Initialize();
                       })
                       .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            }
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Autofac.Module" />
    public class LoggingModule : Autofac.Module
    {
        private static void InjectLoggerProperties([NotNull] object instance)
        {
            var instanceType = instance.GetType();

            // Get all the injectable properties to set.
            // If you wanted to ensure the properties were only UNSET properties,
            // here's where you'd do it.
            var properties = instanceType
                             .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                             .Where(p => p.PropertyType == typeof(ILogger) && p.CanWrite &&
                                         p.GetIndexParameters().Length == 0);

            // Set the properties located.
            foreach (var propToSet in properties)
            {
                propToSet.SetValue(instance, LogManager.GetLogger(instanceType.Name, instanceType), null);
            }
        }

        private static void OnComponentPreparing(object sender, [NotNull] PreparingEventArgs e)
        {
            e.Parameters = e.Parameters.Union(
                                              new[]
                                              {
                                                  new ResolvedParameter(
                                                                        (p, i) => p.ParameterType == typeof(ILogger),
                                                                        (p, i) =>
                                                                            LogManager
                                                                                .GetLogger(p.Member.DeclaringType.Name,
                                                                                           p.Member.DeclaringType)
                                                                       ),
                                              });
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry,
                                                              [NotNull] IComponentRegistration registration)
        {
            // Handle constructor parameters.
            registration.Preparing += OnComponentPreparing;

            // Handle properties.
            registration.Activated += (sender, e) => InjectLoggerProperties(e.Instance);
        }
    }
}