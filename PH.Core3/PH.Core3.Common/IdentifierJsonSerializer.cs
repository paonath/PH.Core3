using System;
using Newtonsoft.Json;
using PH.Core3.Common.Identifiers;

namespace PH.Core3.Common
{
    /// <summary>
    /// The Json.Net Serializer for <see cref="IIdentifier"/>
    /// </summary>
    public class IdentifierJsonSerializer : JsonConverter<IIdentifier>
    {
        sealed class MyId
        {
            public string Uid { get; set; }
        }

        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, IIdentifier value, JsonSerializer serializer)
        {
            if (value is ClaimsPrincipalTenantIdentifier claimsPrincipalTenantIdentifier)
            {
                var converter = new ClaimsPrincipalTenantIdentifierJsonSerializer();
                writer.WriteValue(converter.GetSerializedObject(claimsPrincipalTenantIdentifier));
                
            }
            else
            {
                if (value is ClaimsPrincipalIdentifier claimsPrincipalIdentifier)
                {
                    var converter = new ClaimsPrincipalIdentifierJsonSerializer();
                    writer.WriteValue(converter.GetSerializedObject(claimsPrincipalIdentifier));
                }
                else
                {
                    writer.WriteValue(JsonConvert.SerializeObject(new MyId() {Uid = value.Uid}));
                }
            }
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override IIdentifier ReadJson(JsonReader reader, Type objectType, IIdentifier existingValue, bool hasExistingValue,
                                             JsonSerializer serializer)
        {
            throw new NotSupportedException("Unable to Read from json");
        }
    }
}