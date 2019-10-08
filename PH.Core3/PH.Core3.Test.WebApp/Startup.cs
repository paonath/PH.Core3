using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Multitenant;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PH.Core3.AspNetCoreApi.Filters;
using PH.Core3.Test.WebApp.AutofacModules;
using PH.Core3.TestContext;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace PH.Core3.Test.WebApp
{
    public class Startup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="hostingEnvironment">the hosting environment</param>
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            _environment  = hostingEnvironment;
            Configuration = configuration;
        }

        private IHostingEnvironment _environment;


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
                                                     .UseSqlServer("Server=192.168.3.162\\SQLEXPRESS;Database=ctx_core3;User Id=dev;Password=dev;MultipleActiveResultSets=true")
                                                     .UseLazyLoadingProxies(true)
                                            );
            services.AddIdentity<User, Role>()
                    .AddEntityFrameworkStores<MyContext>()
                    .AddDefaultTokenProviders();
            
            /*
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v0", new Info
                {
                    Version        = "v0",
                    Title          = "ToDo API V0",
                    Description    = "A simple example ASP.NET Core Web API",
                    TermsOfService = "None",
                    Contact = new Contact
                    {
                        Name  = "Paolo Innocenti",
                        Email = "paolo.innocenti@estrobit.com",
                        Url   = "https://estrobit.com/"
                    },
                    //License = new License
                    //{
                    //    Name = "Use under LICX",
                    //    Url  = "https://example.com/license"
                    //},

                });
                //c.SwaggerDoc("v0", new Info { Title = "My API", Version = "v0" });
                c.SwaggerDoc("v1", new Info { Title = "My API", Version = "v1" });
            });
            */

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
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory
                              , Microsoft.AspNetCore.Hosting.IApplicationLifetime appLifetime, IApiVersionDescriptionProvider apiVersionProvider)
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

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            //app.UseSwaggerUI(c =>
            //{
            //    c.SwaggerEndpoint("/swagger/v0/swagger.json", "My API V0");
            //    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            //    //c.DocExpansion("none");
            //    //c.DocExpansion(DocExpansion.None);
            //    c.DocExpansion(DocExpansion.None);

            //});
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
