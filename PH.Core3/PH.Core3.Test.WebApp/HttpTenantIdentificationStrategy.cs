using System;
using System.Text.RegularExpressions;
using Autofac.Multitenant;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using PH.Core3.Common;

namespace PH.Core3.Test.WebApp
{
    public class TenantApiFinder
    {
        private readonly Regex _r = new Regex(@"/\b(v{1}(\d)*)\b");

        public TenantApiFinder()
        {
        }

        public bool IsMatch([CanBeNull] HttpContext context, out string tenant, out int apiVersion)
        {
            tenant     = null;
            apiVersion = 0;

            try
            {
                if (context == null)
                {
                    return false;
                }
                else
                {
                    if (!context.Request.Path.HasValue)
                    {
                        return false;
                    }
                    else
                    {
                        if (_r.IsMatch(context.Request.Path.Value))
                        {
                            var matches = _r.Matches(context.Request.Path.Value);

                            tenant     = matches[0].Groups[1].Value;
                            apiVersion = int.Parse(matches[0].Groups[2].Value);

                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        //public (string Tenant, int ApiVersion) Find([CanBeNull] HttpContext context)
        //{
        //    if (context == null)
        //    {
        //        return (null, Int32.MinValue);
        //    }
        //    else
        //    {
        //        if (!context.Request.Path.HasValue)
        //        {
        //            return (null, Int32.MinValue);
        //        }
        //        else
        //        {

        //            var r = context.Request.Path.Value.Split("/");
        //            var tenantId = r[2];
        //            var apiVersion = int.Parse(tenantId.Replace("v", ""));
        //            return (tenantId, apiVersion);
        //        }
        //    }
        //}
    }

    public class HttpTenantIdentificationStrategy : ITenantIdentificationStrategy
    {
        private TenantApiFinder _finder;

        public HttpTenantIdentificationStrategy(IHttpContextAccessor accessor, TenantApiFinder finder)
        {
            Accessor = accessor;
            _finder  = finder;
        }

        public IHttpContextAccessor Accessor { get; }

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
            var context = this.Accessor.HttpContext;

            bool ident = _finder.IsMatch(context, out string tenant, out int apiVersion);
            if (ident)
            {
                tenantId = tenant;
                return true;
            }
            else
            {
                tenantId = null;
                return false;
            }

            ////throw new System.NotImplementedException();
            //var context = this.Accessor.HttpContext;
            //if (context == null)
            //{
            //    tenantId = null;
            //    return false;
            //}
            //else
            //{
            //    if (!context.Request.Path.HasValue)
            //    {
            //        tenantId = null;
            //        return false;
            //    }
            //    else
            //    {

            //        var r = context.Request.Path.Value.Split("/");
            //        tenantId = r[2];
            //        return true;
            //    }


            //}
        }
    }
}