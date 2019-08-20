using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using PH.Core3.Common;
using PH.Core3.Common.Services.Components;

namespace PH.Core3.EntityFramework.Services.Components
{
    /// <summary>
    /// Abstract service class for work with <see cref="DbContext"/>
    /// </summary>
    /// <typeparam name="TContext">Type of <see cref="DbContext"/></typeparam>
    public abstract class ContextServiceBase<TContext> : ServiceBase where TContext : DbContext
    {
        /// <summary>
        /// Db Context
        /// </summary>
        protected readonly TContext Ctx;

        ///// <summary>
        ///// Tenant Identifier
        ///// </summary>
        //protected string TenantId { get; private set; }

        



        /// <summary>
        /// Initialize a new service instance for work with <see cref="DbContext"/>
        /// </summary>
        /// <param name="coreIdentifier">Identifier</param>
        /// <param name="ctx"><see cref="DbContext"/> db context</param>
        protected ContextServiceBase([NotNull] IIdentifier coreIdentifier, [NotNull] TContext ctx
                                     /*, [NotNull] string tenantId*/)
            : base(coreIdentifier)
        {
            Ctx               = ctx ?? throw new ArgumentNullException(nameof(ctx));
            //CheckAndSetTenant(tenantId);
        }

        //private void CheckAndSetTenant([NotNull] string tenantId)
        //{
        //    if (string.IsNullOrEmpty(tenantId))
        //    {
        //        throw new ArgumentException("Value cannot be null or empty.", nameof(tenantId));
        //    }

        //    if (string.IsNullOrWhiteSpace(tenantId))
        //    {
        //        throw new ArgumentException("Value cannot be null or whitespace.", nameof(tenantId));
        //    }

        //    if(tenantId.Length > 128)
        //    {
        //        throw new ArgumentException(@"Max lenght 128", nameof(tenantId));
        //    }


        //    TenantId = tenantId;
        //}


        
    }
}