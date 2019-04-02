using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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

        public IContainer ApplicationContainer { get; private set; }
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


            this.ApplicationContainer = builder.Build();
            return new AutofacServiceProvider(this.ApplicationContainer);
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
            app.UseMvc();
        }
    }
}
