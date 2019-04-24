using System;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PH.Core3.Test.WebApp
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions( IApiVersionDescriptionProvider provider ) =>
            this.provider = provider;

        public void Configure( SwaggerGenOptions options )
        {
            foreach ( var description in provider.ApiVersionDescriptions )
            {
                try
                {
                    options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e);
                   // throw;
                }
               
            }
        }

        static Info CreateInfoForApiVersion( ApiVersionDescription description )
        {
            var info = new Info()
            {
                Title          = $"Sample API {description.ApiVersion}",
                Version        = description.ApiVersion.ToString(),
                Description    = "A sample application with Swagger, Swashbuckle, and API versioning.",
                Contact        = new Contact() { Name = "Paolo Innocenti", Email = "paonath@gmail.com" },
                //TermsOfService = "Shareware",
                //License        = new License() { Name = "MIT", Url = "https://opensource.org/licenses/MIT" }
            };

            if ( description.IsDeprecated )
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}