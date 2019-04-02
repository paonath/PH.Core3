using Autofac.Multitenant;
using Microsoft.AspNetCore.Http;

namespace PH.Core3.Test.WebApp
{
    public class HttpTenantIdentificationStrategy: ITenantIdentificationStrategy
    {
        public HttpTenantIdentificationStrategy(IHttpContextAccessor accessor)
        {
            Accessor = accessor;
        }

        public IHttpContextAccessor Accessor { get;  }

        /// <summary>
        /// Attempts to identify the tenant from the current execution context.
        /// </summary>
        /// <param name="tenantId">The current tenant identifier.</param>
        /// <returns>
        /// <see langword="true" /> if the tenant could be identified; <see langword="false" />
        /// if not.
        /// </returns>
        public bool TryIdentifyTenant(out object tenantId)
        {
            //throw new System.NotImplementedException();
            var context = this.Accessor.HttpContext;
            if (context == null)
            {
                tenantId = null;
                return false;
            }
            else
            {
                if (!context.Request.Path.HasValue)
                {
                    tenantId = null;
                    return false;
                }
                else
                {
                    
                    var r = context.Request.Path.Value.Split("/");
                    tenantId = r[2];
                    return true;
                }

                
            }


            

        }
    }
}