using System;
using System.Text.Json.Serialization;


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
        //[JsonProperty(PropertyName = @"_guid")]
        //[JsonName("birthdate")]
        [JsonPropertyName(@"_guid")]
        public Guid BaseIdentifierGuid {get;}

        /// <summary>
        /// Utc Date and Time init of current identifier
        /// </summary>
        //[JsonProperty(PropertyName = @"_utc")]
        [JsonPropertyName(@"_guid")]
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