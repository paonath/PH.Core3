using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PH.Core3.AspNetCoreApi.Filters;
using PH.Core3.Test.WebApp.AutofacModules;
using PH.Core3.TestContext;

namespace PH.Core3.Test.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static MultitenantContainer ApplicationContainer { get; set; }
        
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IPrincipal>(
                                              provider => provider
                                                          .GetService<IHttpContextAccessor>().HttpContext?.User);


            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services
                .AddMvc(options => options.Filters.Add<InterceptionAttributeFilter>())
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.DateTimeZoneHandling  = DateTimeZoneHandling.Utc;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices();


            var contextConnectionString = Configuration.GetConnectionString("MySqlConnection");

            services.AddDbContext<MyContext>(options =>
                                               options
                                                   .UseMySql(contextConnectionString)
                                                   .UseLazyLoadingProxies(true)
                                          );
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<MyContext>()
                    .AddDefaultTokenProviders();
            

            var builder = new ContainerBuilder();

            builder.Populate(services);
            //builder.RegisterModule<AutofacModules.MainwebModule>(contextConnectionString);
            builder.RegisterModule(new MainwebModule(contextConnectionString));


            var container = builder.Build();
            var strategy = new HttpTenantIdentificationStrategy(container.Resolve<IHttpContextAccessor>());



            var multitenantContainer = new MultitenantContainer(strategy, container);


            ApplicationContainer = multitenantContainer;

            return new AutofacServiceProvider(multitenantContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory
                              , Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseMvc();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                                name: "areaRoutev1",
                                template: "{area=v1}/api/{controller=Home}/{action=Get}/{id?}");
                /*
                routes.MapRoute(
                                name: "fileRoute",
                                template: "Files/{action=Attachments}/{id}");
                //http://localhost:5001/Files/Attachments/08d5fc393bd20a13aa02e3e1dcbdb95b-ReSharper_DefaultKeymap_VSscheme.pdf
                */
                routes.MapRoute(
                                name: "default",
                                template: "{controller=Home}/{action=Index}/{id?}");
               
            });

            app.UseStaticFiles();
        }
    }
}
