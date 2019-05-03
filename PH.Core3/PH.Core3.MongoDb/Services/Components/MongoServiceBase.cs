using System;
using System.Collections.Generic;
using System.Text;

namespace PH.Core3.MongoDb.Services.Components
{
    public abstract class MongoServiceBase
    {
        /// <summary>
        /// Tenant Identifier
        /// </summary>
        protected string TenantId { get; private set; }
    }
}
