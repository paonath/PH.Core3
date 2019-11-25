﻿using System;
using Newtonsoft.Json;

namespace PH.Core3.Common.Identifiers
{
    /// <summary>
    /// The Json.Net Serializer for <see cref="ClaimsPrincipalIdentifier"/>
    /// </summary>
    public class ClaimsPrincipalIdentifierJsonSerializer : JsonConverter<PH.Core3.Common.Identifiers.ClaimsPrincipalIdentifier>
    {
        sealed class MyId
        {
            public string Uid { get; set; }
            public string Name { get; set; }
            public bool IsAuthenticated { get; set; }
        }


        /// <summary>Writes the JSON representation of the object.</summary>
        /// <param name="writer">The <see cref="T:Newtonsoft.Json.JsonWriter" /> to write to.</param>
        /// <param name="value">The value.</param>
        /// <param name="serializer">The calling serializer.</param>
        public override void WriteJson(JsonWriter writer, ClaimsPrincipalIdentifier value, JsonSerializer serializer)
        {
            var v = GetSerializedObject(value);
            writer.WriteValue(v);
        }

        /// <summary>Gets the serialized object.</summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public string GetSerializedObject(ClaimsPrincipalIdentifier value)
        {
            return JsonConvert.SerializeObject(new MyId()
            {
                Uid             = value.Uid, Name = value.Name,
                IsAuthenticated = value.Principal?.Identity?.IsAuthenticated ?? false
            });
        }

        /// <summary>Reads the JSON representation of the object.</summary>
        /// <param name="reader">The <see cref="T:Newtonsoft.Json.JsonReader" /> to read from.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="existingValue">The existing value of object being read. If there is no existing value then <c>null</c> will be used.</param>
        /// <param name="hasExistingValue">The existing value has a value.</param>
        /// <param name="serializer">The calling serializer.</param>
        /// <returns>The object value.</returns>
        public override ClaimsPrincipalIdentifier ReadJson(JsonReader reader, Type objectType, ClaimsPrincipalIdentifier existingValue,
                                                           bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException("Unable to Read from json");
        }
    }
}