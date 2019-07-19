using System.Linq;
using System.Reflection;
using System.Text;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PH.Core3.Common;
using PH.Core3.UnitOfWork;

namespace PH.Core3.AspNetCoreApi.Filters
{
    /// <summary>
    /// Filter for logging Actions on Controllers.
    ///
    /// <para>All actions must be decorated with <see cref="LogActionAttribute">LogActionAttribute</see></para>
    /// 
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ActionFilterAttribute" />
    public class InterceptionAttributeFilter: ActionFilterAttribute
    {
        /// <summary>The identifier</summary>
        protected readonly IIdentifier Identifier;

        /// <summary>The logger</summary>
        protected readonly ILogger<InterceptionAttributeFilter> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptionAttributeFilter"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="logger">The logger.</param>
        public InterceptionAttributeFilter(IIdentifier identifier, ILogger<InterceptionAttributeFilter> logger)
        {
            Identifier = identifier;
            Logger     = logger;
        }

        /// <summary>Logs the begin execution of a MVC Action.</summary>
        /// <param name="context">The context.</param>
        protected virtual void LogExecuting([CanBeNull] ActionExecutingContext context)
        {
            if(null == context)
            {
                return;
            }


            var actionDescriptor = context.ActionDescriptor.DisplayName;

            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            if (null != descriptor)
            {
                var attributes = descriptor.MethodInfo.CustomAttributes;

                var skip = descriptor.MethodInfo.GetCustomAttribute<SkipInterceptionLogAttribute>();
                if (null != skip)
                {
                    return;
                }
                
                var loggingAttr = descriptor.MethodInfo.GetCustomAttribute<LogActionAttribute>();
                if (null == loggingAttr)
                {
                    return;
                }
                else
                {
                    StringBuilder msgBuilder = new StringBuilder();
                    msgBuilder.Append(loggingAttr.Prefix);
                    if (loggingAttr.LogIpCaller)
                    {
                        var ip = GetIp(context.HttpContext);
                        if(!string.IsNullOrEmpty(ip))
                        {
                            ip = $"FROM '{ip}' ==> ";
                        }

                        msgBuilder.Append($" {ip}");
                    }
                    var validModelState = context.ModelState.IsValid;
                    msgBuilder.Append($" {nameof(actionDescriptor)}: '{actionDescriptor}'; {nameof(validModelState)}: '{validModelState}'");

                    if (loggingAttr.LogActionArguments)
                    {
                        var data = JsonConvert.SerializeObject(context.ActionArguments.ToDictionary(pair => pair));
                        msgBuilder.Append($" {nameof(data)}; '{data}'");

                    }

                    if (loggingAttr.PostfixMessage != string.Empty)
                    {
                        msgBuilder.Append($" {loggingAttr.PostfixMessage}");
                    }

                    Logger.Log(loggingAttr.LogLevel, msgBuilder.ToString());

                }

            }
            
           
        }

        /// <summary>Gets the ip from HttpContext.</summary>
        /// <param name="ctx">The HttpContext.</param>
        /// <returns></returns>
        [CanBeNull]
        protected virtual string GetIp([CanBeNull] HttpContext ctx)
        {
            if (ctx == null)
            {
                return null;
            }

            var remoteIpAddress = $"{ctx.Connection.RemoteIpAddress}:{ctx.Connection.RemotePort}" ;
            return remoteIpAddress;
        }

        /// <summary>
        /// </summary>
        /// <param name="context"></param>
        /// <inheritdoc />
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            LogExecuting(context);
            base.OnActionExecuting(context);
        }



    }
}
