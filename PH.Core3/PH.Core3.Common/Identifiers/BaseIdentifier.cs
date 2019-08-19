using System;
using Newtonsoft.Json;

namespace PH.Core3.Common.Identifiers
{
    /// <summary>
    /// Abstraction of Unique Identifier across Scope
    /// </summary>
    public abstract class BaseIdentifier
    {
        /// <summary>
        /// Guid
        /// </summary>
        [JsonProperty(PropertyName = @"_guid")]
        public Guid BaseIdentifierGuid {get;}

        /// <summary>
        /// Utc Date and Time init of current identifier
        /// </summary>
        [JsonProperty(PropertyName = @"_utc")]
        public DateTime UtcGenerated { get; }


        /// <summary>
        /// 
        /// </summary>
        protected BaseIdentifier()
        {
            BaseIdentifierGuid = Guid.NewGuid();
            UtcGenerated       = DateTime.UtcNow;
        }
        
    }
}