using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Services.Path;

namespace PH.Core3.AspNetCoreApi.Services.Components
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly ITempDataProvider _tempDataProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly IWebPathTranslator _webPathTranslator;

        private readonly ILogger<ViewRenderService> _logger;


        public ViewRenderService(IRazorViewEngine razorViewEngine,
                                 ITempDataProvider tempDataProvider,
                                 IServiceProvider serviceProvider, IWebPathTranslator webPathTranslator, ILogger<ViewRenderService> logger = null)
        {
            _razorViewEngine   = razorViewEngine;
            _tempDataProvider  = tempDataProvider;
            _serviceProvider   = serviceProvider;
            _webPathTranslator = webPathTranslator;
            _logger = logger;
        }

        /// <summary>
        /// Render a string from CSHTML View and Model
        /// </summary>
        /// <param name="viewName">View Name</param>
        /// <param name="model">Model instance</param>
        /// <returns>string</returns>
        public async Task<string> RenderToStringAsync(string viewWebPath, object model)
        {
            var httpContext   = new DefaultHttpContext { RequestServices = _serviceProvider };
            var actionContext = new ActionContext(httpContext, new RouteData(), new ActionDescriptor());

            using (var sw = new StringWriter())
            {
                try
                {
                    var absPath  = _webPathTranslator.ToFileSystemPath("~/");
                    var viewPath = _webPathTranslator.ToFileSystemPath(viewWebPath);
                    var vv       = viewWebPath.Replace("~", "");

                    var vResult = _razorViewEngine.GetView(absPath, vv, false);


              
                    if (vResult.View == null)
                    {
                        throw new ArgumentNullException($"{vv} does not match any available view");
                    }

                    var viewDictionary = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
                    {
                        Model = model
                        
                    };

                    var viewContext = new ViewContext(
                                                      actionContext,
                                                      vResult.View,
                                                      viewDictionary,
                                                      new TempDataDictionary(actionContext.HttpContext, _tempDataProvider),
                                                      sw,
                                                      new HtmlHelperOptions()
                                                     );
                    
                    await vResult.View.RenderAsync(viewContext);
                    return sw.ToString();

                }
                catch (Exception e)
                {
                    _logger?.LogCritical(e,$"Unable to render view '{viewWebPath}': {e.Message}");
                    throw;
                }

               
            }
        }

        public Task<string> RenderToStringAsync<TModel>(string viewName, TModel model) where TModel : class
        {
            return RenderToStringAsync(viewName, model as object);
        }
    }
}