using System.Linq;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PH.Core3.Common;

namespace PH.Core3.AspNetCoreApi.Filters
{
    /// <summary>
    /// Action filter for logging executing action
    /// </summary>
    public class InterceptionAttributeFilter: ActionFilterAttribute
    {
        /// <summary>
        /// Id
        /// </summary>
        protected readonly IIdentifier Identifier;
        /// <summary>
        /// Logger
        /// </summary>
        protected readonly ILogger<InterceptionAttributeFilter> Logger;

        /// <summary>
        /// Init new instance
        /// </summary>
        /// <param name="identifier">identifier</param>
        /// <param name="logger">logger</param>
        public InterceptionAttributeFilter(IIdentifier identifier, ILogger<InterceptionAttributeFilter> logger)
        {
            Identifier = identifier;
            Logger = logger;
        }

        /// <summary>
        /// log executing action if not marked with SkipInterceptionLogAttribute
        /// </summary>
        /// <param name="context">action context</param>
        protected virtual void LogExecuting([CanBeNull] ActionExecutingContext context)
        {
            if(null == context)
                return;

           

            var actionDescriptor = context.ActionDescriptor.DisplayName;

            var descriptor = (ControllerActionDescriptor)context.ActionDescriptor;
            if (null != descriptor)
            {
                var attributes = descriptor.MethodInfo.CustomAttributes;
                if (attributes.Any(a => a.AttributeType == typeof(SkipInterceptionLogAttribute)))
                    return;
            }

            var ip = GetIp(context.HttpContext);
            if(!string.IsNullOrEmpty(ip))
                ip = $"FROM '{ip}' ";

            var data            = JsonConvert.SerializeObject(context.ActionArguments.ToDictionary(pair => pair));
            var validModelState = context.ModelState.IsValid;

            string msg =
                $"CALL {ip}==> {nameof(actionDescriptor)}: '{actionDescriptor}'; {nameof(validModelState)}: '{validModelState}' {nameof(data)}; '{data}'";
            
            
            Logger.LogDebug(msg);
        }

        /// <summary>
        /// Get caller ip
        /// </summary>
        /// <param name="ctx">http context</param>
        /// <returns>ip adress as string</returns>
        [CanBeNull]
        protected virtual string GetIp([CanBeNull] HttpContext ctx)
        {
            if (ctx == null)
                return null;

            var remoteIpAddress = $"{ctx.Connection.RemoteIpAddress}:{ctx.Connection.RemotePort}" ;
            return remoteIpAddress;
        }


        /// <summary>
        /// Log Action Executing
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
                LogExecuting(context);




            base.OnActionExecuting(context);
        }



    }
}