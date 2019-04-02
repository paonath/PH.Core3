using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Web;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace PH.Core3.Test.WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
           // BuildWebHost(args).Run();
           // NLog: setup the logger first to catch all errors
           var logger = NLog.Web.NLogBuilder.ConfigureNLog(GetNLogCOnfiguration()).GetCurrentClassLogger();
           
           try
           {
               logger.Info("Init App");
               var fullName = typeof(Program).GetTypeInfo().Assembly.FullName;
               logger.Info($"Assembly: {fullName}");


               BuildWebHost(args).Run(); 
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

        //public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        //    WebHost.CreateDefaultBuilder(args)
        //           .UseStartup<Startup>();

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                   .ConfigureAppConfiguration((builderContext, config) =>
                   {
                       var env = builderContext.HostingEnvironment;

                       config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: false, reloadOnChange: true);
                   })

                   #region new Nlog
                   .ConfigureLogging(logging =>
                   {
                       logging.ClearProviders();
                       logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                   })
                   .UseNLog(new NLogAspNetCoreOptions(){  CaptureMessageTemplates = true, CaptureMessageProperties =true, IncludeScopes = true , IgnoreEmptyEventId = true }) // NLog: setup NLog for Dependency injection
                   #endregion

                   .UseStartup<Startup>()
                   .UseAutofacMultitenantRequestServices(() => Startup.ApplicationContainer)
                   .UseUrls("http://localhost:5000", "https://localhost:5001")
                   .Build();


        public static LoggingConfiguration GetNLogCOnfiguration()
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

            fileTarget.FileName = "${basedir}/wwwroot/logs/log.log";

            fileTarget.Layout = commonLayout;


            fileTarget.ArchiveNumbering = ArchiveNumberingMode.DateAndSequence;
            fileTarget.ArchiveEvery     = FileArchivePeriod.Day;
            //fileTarget.KeepFileOpen = true;
            fileTarget.AutoFlush                    = true;
            fileTarget.ArchiveDateFormat            = "dd-MM-yyyy";
            fileTarget.ArchiveOldFileOnStartup      = true;
            fileTarget.ArchiveFileName              = "${basedir}/wwwroot/logs/log.{#}.log.zip";
            fileTarget.EnableArchiveFileCompression = true;


            var blFileTarget = new FileTarget();
            config.AddTarget("blFileTarget", blFileTarget);


            blFileTarget.FileName = "${basedir}/wwwroot/logs/_bl-log.log";
            blFileTarget.Layout   = fileTarget.Layout;


            blFileTarget.ArchiveNumbering             = ArchiveNumberingMode.DateAndSequence;
            blFileTarget.ArchiveEvery                 = FileArchivePeriod.Day;
            blFileTarget.AutoFlush                    = true;
            blFileTarget.ArchiveDateFormat            = "dd-MM-yyyy";
            blFileTarget.ArchiveOldFileOnStartup      = true;
            blFileTarget.ArchiveFileName              = "${basedir}/wwwroot/logs/_bl-log.{#}.log.zip";
            blFileTarget.EnableArchiveFileCompression = true;


            var blackHole = new NullTarget("blackHole");
            config.AddTarget("blackHole", blackHole);

            // Step 4. Define rules
            var rule1 = new LoggingRule("*", NLog.LogLevel.Debug, consoleTarget);
            config.LoggingRules.Add(rule1);

            var rule2 = new LoggingRule("ES.*", NLog.LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rule2);
            var rulePH = new LoggingRule("PH.*", NLog.LogLevel.Debug, fileTarget);
            config.LoggingRules.Add(rulePH);


            var ruleMs = new LoggingRule("Microsoft.*", NLog.LogLevel.Trace, blackHole);
            config.LoggingRules.Add(ruleMs);


            var blRule = new LoggingRule("PH.Core3.Test.*", NLog.LogLevel.Debug, blFileTarget);
            blRule.LoggerNamePattern = "PH.Core3.Test.*";

            //config.LoggingRules.Add(blRule);


            var dbg = fileTarget.Layout;


            return config;
        }

        public static void ConfigureNlogger()
        {
            // Step 5. Activate the configuration
            LogManager.Configuration = GetNLogCOnfiguration();
        }
    }
}
