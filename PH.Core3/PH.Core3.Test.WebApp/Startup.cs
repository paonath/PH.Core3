using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Text.Json;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PH.Core3.AspNetCoreApi.Filters;
using PH.Core3.Common.Json;
using PH.Core3.Test.WebApp.AutofacModules;
using PH.Core3.TestContext;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using JsonSerializer = Newtonsoft.Json.JsonSerializer;

namespace PH.Core3.Test.WebApp
{

    

    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">the hosting environment</param>
        public Startup(IConfiguration configuration, IWebHostEnvironment hostingEnvironment)
        {
            _environment  = hostingEnvironment;
            Configuration = configuration;
        }

        private IWebHostEnvironment _environment;


        public static MultitenantContainer ApplicationContainer { get; set; }
        
        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        [NotNull]
        public IServiceProvider ConfigureServices([NotNull] IServiceCollection services)
        {
            #region base DI

            services.AddCors();

            services.AddTransient<IPrincipal>(
                                              provider => provider
                                                          .GetService<IHttpContextAccessor>().HttpContext?.User);


            services.AddSingleton<ILoggerFactory, LoggerFactory>();

            services.AddResponseCompression(options => options.Providers.Add<GzipCompressionProvider>());

            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);
            services
                .AddMvc(options =>
                {
                    options.Filters.Add<InterceptionAttributeFilter>();
                    options.EnableEndpointRouting = false;

                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new DateTimeUtcConverter());

                    //options.SerializerSettings.DateTimeZoneHandling  = DateTimeZoneHandling.Utc;
                    //options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddControllersAsServices();


            var contextConnectionString = Configuration.GetConnectionString("MySqlConnection");

            services.AddDbContext<MyContext>(options =>
                                                 options
                                                     .UseSqlServer("Server=192.168.3.83\\SQLEXPRESS;Database=ctx_core3;User Id=dev;Password=dev;MultipleActiveResultSets=true")
                                                     .UseLazyLoadingProxies(true)
                                            );
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<MyContext>()
                    .AddDefaultTokenProviders();
            
            

            services.AddScoped<TenantApiFinder, TenantApiFinder>();

            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionSelector = new PerRouteApiVersionSelector(new TenantApiFinder());
                //options.RouteConstraintName = "area";
            });
            services.AddVersionedApiExplorer( options => options.GroupNameFormat = "'v'VVV" );
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen();

            #endregion

            

            #region Autofac

            var builder = new ContainerBuilder();

            builder.Populate(services);
            //builder.RegisterModule<AutofacModules.MainwebModule>(contextConnectionString);
            builder.RegisterModule(new MainwebModule(contextConnectionString));


            var container = builder.Build();
            var strategy  = new HttpTenantIdentificationStrategy(container.Resolve<IHttpContextAccessor>(), container.Resolve<TenantApiFinder>());



            var multitenantContainer = new MultitenantContainer(strategy, container);


            ApplicationContainer = multitenantContainer;

            return new AutofacServiceProvider(multitenantContainer);

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory
                              , Microsoft.Extensions.Hosting.IHostApplicationLifetime appLifetime, IApiVersionDescriptionProvider apiVersionProvider)
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

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger( );

            
            app.UseSwaggerUI(
                             options =>
                             {
                                 foreach ( var description in apiVersionProvider.ApiVersionDescriptions )
                                 {
                                     options.SwaggerEndpoint(
                                                             $"/swagger/{description.GroupName}/swagger.json",
                                                             description.GroupName.ToUpperInvariant() );
                                     options.DocExpansion(DocExpansion.None);
                                 }
                             } );

            //app.UseRouting();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                                name: "areaRoutev1",
                                template: "{area=v1}/api/{controller=Home}/{action=Get}/{id?}");
                
                routes.MapRoute(
                                name: "default",
                                template: "{controller=Home}/{action=Index}/{id?}");
               
            });

            app.UseStaticFiles();
        }


    }

    public class PerRouteApiVersionSelector : IApiVersionSelector
    {
        private readonly TenantApiFinder _finder;

        public PerRouteApiVersionSelector(TenantApiFinder finder)
        {
            _finder = finder;
        }

        /// <summary>
        /// Selects an API version given the specified HTTP request and API version information.
        /// </summary>
        /// <param name="request">The current <see cref="T:Microsoft.AspNetCore.Http.HttpRequest">HTTP request</see> to select the version for.</param>
        /// <param name="model">The <see cref="T:Microsoft.AspNetCore.Mvc.Versioning.ApiVersionModel">model</see> to select the version from.</param>
        /// <returns>The selected <see cref="T:Microsoft.AspNetCore.Mvc.ApiVersion">API version</see>.</returns>
        [NotNull]
        public ApiVersion SelectVersion([NotNull] HttpRequest request, ApiVersionModel model)
        {
            if (_finder.IsMatch(request.HttpContext, out var tenant, out var version))
            {
                return new ApiVersion(version, 0);
            }
            else
            {
                throw new ArgumentException("Unable to determine API Version from Request Context", nameof(request));
            }
        }
    }

    public class SwaggerDefaultValues : IOperationFilter
    {
        /// <summary>
        /// Applies the filter to the specified operation using the given context.
        /// </summary>
        /// <param name="operation">The operation to apply the filter to.</param>
        /// <param name="context">The current operation filter context.</param>
        public void Apply( [NotNull] Operation operation, [NotNull] OperationFilterContext context )
        {
            var apiDescription = context.ApiDescription;

            operation.Deprecated = apiDescription.IsDeprecated();
            


            if ( operation.Parameters == null )
            {
                return;
            }

            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/412
            // REF: https://github.com/domaindrivendev/Swashbuckle.AspNetCore/pull/413
            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

                if (parameter.Description == null)
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Default == null)
                {
                    parameter.Default = description.DefaultValue;
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }


}
