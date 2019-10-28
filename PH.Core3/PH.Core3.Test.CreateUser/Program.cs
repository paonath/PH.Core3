using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using PH.Core3.Common;
using PH.Core3.Common.Services.Components.Crud;
using PH.Core3.TestContext;
using PH.UowEntityFramework.EntityFramework.Extensions;
using PH.UowEntityFramework.UnitOfWork;

namespace PH.Core3.Test.CreateUser
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(l =>
            {
                //l.AddConsole().AddFilter(level => level >= LogLevel.Trace);

                //l.AddDebug();
            });

            serviceCollection.AddLogging();
            serviceCollection.AddSingleton<ILoggerFactory, LoggerFactory>();


            serviceCollection.AddDbContext<MyContext>(options =>
                                                          options
                                                             // .UseMySql("server=localhost;database=ctx_core3;user=dev;password=dev;SslMode=none")
                                                             .UseSqlServer("Server=192.168.3.162\\SQLEXPRESS;Database=ctx_core3;User Id=dev;Password=dev;MultipleActiveResultSets=true")
                                                              .UseLazyLoadingProxies(true)
                                                     );

            serviceCollection.AddIdentity<User, Role>()
                             .AddEntityFrameworkStores<MyContext>()
                             .AddDefaultTokenProviders();


            // BuildWebHost(args).Run();
            // NLog: setup the logger first to catch all errors
            var logger = NLog.Web.NLogBuilder.ConfigureNLog(NlogConfig.GetNLogCOnfiguration()).GetCurrentClassLogger();

            try
            {
                logger.Info("Init App");
                ShowThreadInformation("Init App");

                var fullName = typeof(Program).GetTypeInfo().Assembly.FullName;
                logger.Info($"Assembly: {fullName}");

                using (var scope = Init(serviceCollection))
                {
                    PerformCodeCaller(scope).GetAwaiter().GetResult();
                    ShowThreadInformation("PerformCodeCaller result");
                }
            }
            catch (Exception ex)
            {
                //NLog: catch setup errors
                logger.Error(ex, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }


        private static Object lockObj = new Object();

        private static void ShowThreadInformation(String taskName)
        {
            String msg    = null;
            Thread thread = Thread.CurrentThread;
            lock(lockObj) {
                msg = $"{taskName} thread information\n" +
                      $"   Background: {thread.IsBackground}\n" +
                      $"   Thread Pool: {thread.IsThreadPoolThread}\n" +
                      $"   Thread ID: {thread.ManagedThreadId}\n";
            }
            Console.WriteLine(msg);
        }


        private static async Task<User> CreateUserAsync(User user, string password, ILifetimeScope scope)
        {
            ShowThreadInformation("CreateUserAsync - Begin");
            var userManager = scope.Resolve<ApplicationUserManager>();
            var ty = await userManager.CreateAsync(user, password).ConfigureAwait(false);
            
            ShowThreadInformation("CreateUserAsync - End");
            return user;
        }

        private static async Task<int> MySaveChanges(MyContext ctx)
        {
            ShowThreadInformation("MySaveChanges - Begin");
            var c = await ctx.SaveChangesAsync().ConfigureAwait(false);
            await WaitMe().ConfigureAwait(false);
            ShowThreadInformation("MySaveChanges - End");
            return c;
        }


        private static async Task WaitMe()
        {
            ShowThreadInformation("WaitMe");
            await Task.Delay(1000);

            return;
        }

        private static async Task<(string Username, string Password)> GetUserAndPwdFromConsole()
        {
            await WaitMe().ConfigureAwait(false);

            ShowThreadInformation("PerformInternal - Provide Email");
            Console.WriteLine("Provide Email");
            var user = Console.ReadLine();

            await WaitMe().ConfigureAwait(false);
            Console.WriteLine("Provide Password");
            var password = Console.ReadLine();

            (string Username, string Password) r = (user, password);

            return r;
        }

        private static async Task<int> PerformInternal(ILifetimeScope scope)
        {
            await WaitMe().ConfigureAwait(false);
            int c = 0;

            var user = await GetUserAndPwdFromConsole().ConfigureAwait(false);


            var ctx = scope.Resolve<MyContext>();

           
            await WaitMe().ConfigureAwait(false);

            var u = await CreateUserAsync(new User() {Email = user.Username, UserName = user.Username}, user.Password, scope)
                        .ConfigureAwait(false);

            ShowThreadInformation("PerformInternal > CreateUserAsync - End");


            var tyu = await MySaveChanges(ctx).ConfigureAwait(false);

            ShowThreadInformation("PerformInternal - SaveChangesAsync 1");
            c += tyu;


            u.AccessFailedCount = 2;
            await ctx.Users.UpdateAsync(u).ConfigureAwait(false);
            ShowThreadInformation("PerformInternal - UpdateAsync");
            int full = await MySaveChanges(ctx).ConfigureAwait(false);

            ShowThreadInformation("PerformInternal - SaveChangesAsync 2");

            c += full;

            return c;
        }


        private static async Task PerformCodeCaller(ILifetimeScope scope)
        {
            ShowThreadInformation("PerformCodeCaller");
             await PerformCode(scope);

        }

        private static async Task PerformCode(ILifetimeScope scope)
        {
            ShowThreadInformation("PerformCode");
            await WaitMe().ConfigureAwait(false);
            using (var uow = scope.Resolve<IUnitOfWork>())
            {
                //var dbg = scope.Resolve<DataService>();
                //var t = await dbg.AddAsync(new NewTestDataDto() {data = "Test"});
                //uow.Commit();



                var ctx = scope.Resolve<MyContext>();
                ctx.Initialize();

                //ctx.TenantName = "SGURZ";
                //ctx.TenantName = "Pippo Pippottolo";

                var userManager = scope.Resolve<ApplicationUserManager>();

                var x = await PerformInternal(scope).ConfigureAwait(false);
                await WaitMe().ConfigureAwait(false);

                uow.Commit("create user");
            }
        }


        private static ILifetimeScope Init(ServiceCollection serviceCollection)
        {
            var containerBuilder = new ContainerBuilder();


            containerBuilder.Populate(serviceCollection);


            containerBuilder.Register(c =>
                            {
                                string iid = $"{Guid.NewGuid():N}";
                                MappedDiagnosticsLogicalContext.Set("IID", iid);

                                return new PH.Core3.Common.Identifiers.Identifier(iid);
                            })
                            .AsSelf()
                            .As<IIdentifier>()
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();


            containerBuilder.Register(c =>
                            {
                                var context = c.Resolve<MyContext>();
                                context.Author     = "Console App";
                                context.Identifier = c.Resolve<IIdentifier>().Uid;
                                context.UowLogger = c.Resolve<ILogger<IUnitOfWork>>();
                                context.Initialize();

                                return context;

                            })
                            //.AsSelf()
                            .As<IUnitOfWork>()
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();


            containerBuilder.RegisterType<PH.PicoCrypt2.AesCrypt>()
                            .AsSelf()
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope();

            var authServices = typeof(ApplicationUserManager).Assembly.GetTypes()
                                                             .Where(t => t.IsAbstract == false
                                                                   )
                                                             .ToArray();
            foreach (var serviceType in authServices)
            {
                containerBuilder.RegisterType(serviceType)
                                .AsSelf()
                                .AsImplementedInterfaces()
                                .InstancePerLifetimeScope()
                                .OnActivated(e => { })
                                .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);
            }
            containerBuilder.Register(c => new TransientCrudSettings(c.Resolve<IIdentifier>(),
                                                            c.Resolve<ILogger<TransientCrudSettings>>(), true, true,
                                                            true, true)).AsSelf().AsImplementedInterfaces()
                   .InstancePerLifetimeScope();


            containerBuilder.RegisterType<DataService>().AsSelf()
                            .AsImplementedInterfaces()
                            .InstancePerLifetimeScope()
                            .OnActivated(e => { })
                            .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            var container       = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);
            return container.BeginLifetimeScope();
        }
    }


    internal static class NlogConfig
    {
        [NotNull]
        public static LoggingConfiguration GetNLogCOnfiguration(string apiVersion = "")
        {
            //%date %-5level [%property{ExecutingCtx}] - %message | %stacktrace{5} | [%logger ]%newline" 

            var layout =
                @"${longdate:universalTime=true} ${pad:padding=5:inner=${level:uppercase=true}} [${pad:padding=5:inner=${mdlc:item=IID}}] - ${message} ${when:when=length('${exception}')>0:Inner=[BEGIN_EXCEPTION_}${exception:format=toString,Data:maxInnerExceptionLevel=10}${when:when=length('${exception}')>0:Inner=_END_EXCEPTION]} | ${event-properties:item=EventId_Id} ${ndlc:uppercase=true:separator= => } | [${callsite:fileName=true:methodName=true:cleanNamesOfAsyncContinuations=true:cleanNamesOfAnonymousDelegates=true:includeSourcePath=false}] [${logger:shortName=false}] [$END$]";


            // Step 1. Create configuration object 
            var config = new LoggingConfiguration();

            // Step 2. Create targets and add them to the configuration 
            var consoleTarget = new ColoredConsoleTarget();
            config.AddTarget("console", consoleTarget);

            var fileTarget = new FileTarget();
            config.AddTarget("file", fileTarget);


            var commonLayout  = layout;
            var consoleLayout = commonLayout.Replace("[$END$]", "");

            consoleTarget.Layout = consoleLayout;

            fileTarget.FileName = $"${{basedir}}/logs/log{apiVersion}.log";

            fileTarget.Layout = commonLayout;


            fileTarget.ArchiveNumbering = ArchiveNumberingMode.DateAndSequence;
            fileTarget.ArchiveEvery     = FileArchivePeriod.Day;
            //fileTarget.KeepFileOpen = true;
            fileTarget.AutoFlush                    = true;
            fileTarget.ArchiveDateFormat            = "dd-MM-yyyy";
            fileTarget.ArchiveOldFileOnStartup      = true;
            fileTarget.ArchiveFileName              = $"${{basedir}}/logs/log{apiVersion}{{#}}.log.zip";
            fileTarget.EnableArchiveFileCompression = true;


            var blFileTarget = new FileTarget();
            config.AddTarget("blFileTarget", blFileTarget);


            blFileTarget.FileName = $"${{basedir}}/logs/BL-log{apiVersion}.log";
            blFileTarget.Layout   = fileTarget.Layout;


            blFileTarget.ArchiveNumbering             = ArchiveNumberingMode.DateAndSequence;
            blFileTarget.ArchiveEvery                 = FileArchivePeriod.Day;
            blFileTarget.AutoFlush                    = true;
            blFileTarget.ArchiveDateFormat            = "dd-MM-yyyy";
            blFileTarget.ArchiveOldFileOnStartup      = true;
            blFileTarget.ArchiveFileName              = $"${{basedir}}/logs/BL-log{apiVersion}{{#}}.log.zip";
            blFileTarget.EnableArchiveFileCompression = true;


            var blackHole = new NullTarget("blackHole");
            config.AddTarget("blackHole", blackHole);

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", NLog.LogLevel.Trace, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("*", NLog.LogLevel.Trace, fileTarget);
            config.LoggingRules.Add(rule2);

            //var rulePH = new LoggingRule("PH.*", NLog.LogLevel.Debug, fileTarget);
            //config.LoggingRules.Add(rulePH);


            //var ruleMs = new LoggingRule("Microsoft.*", NLog.LogLevel.Trace, blackHole);
            //config.LoggingRules.Add(ruleMs);


            //var blRule = new LoggingRule("PH.Core3.*", NLog.LogLevel.Debug, blFileTarget);
            //blRule.LoggerNamePattern = "PH.Core3.Test.*";

            ////config.LoggingRules.Add(blRule);


            return config;
        }

        public static void ConfigureNlogger()
        {
            // Step 5. Activate the configuration
            LogManager.Configuration = GetNLogCOnfiguration();
        }
    }

    public class MySettingsConfig
    {
        public string ConnectionString { get; set; }
    }
}