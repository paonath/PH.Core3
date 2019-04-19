﻿using System;
using JetBrains.Annotations;
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
            UtcGenerated = DateTime.UtcNow;
        }
        
    }


    /// <summary>
    /// Unique Identifier across Scope
    /// </summary>
    public class Identifier : BaseIdentifier, IIdentifier
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="uid"></param>
        public Identifier([NotNull] string uid)
            :base()
        {
            Uid = uid;
        }

        /// <summary>
        /// Unique Identifier
        /// </summary>
        public string Uid { get; }

        /// <summary>Determines whether the specified object is equal to the current object.</summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object  is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            return obj?.GetHashCode() == GetHashCode();
        }

        /// <summary>Serves as the default hash function.</summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return ($"{typeof(Identifier)} {ToString()}").GetHashCode();
        }

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return $"Uid: '{Uid}' - Generated on '{UtcGenerated:O}' - Guid '{BaseIdentifierGuid}'";
        }

        
    }
}