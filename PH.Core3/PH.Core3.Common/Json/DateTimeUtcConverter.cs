using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;
using JetBrains.Annotations;

namespace PH.Core3.Common.Json
{
    /// <summary>
    /// Serialize and Deserialize JSON Datetime as UTC
    /// </summary>
    /// <seealso cref="DateTime" />
    /// <example>
    /// <code>
    /// ...
    /// services
    /// .AddMvc()
    /// .AddJsonOptions(options =>
    /// {
    /// options.JsonSerializerOptions.Converters.Add(new DateTimeUtcConverter());
    /// }) 
    /// ...
    /// </code>
    /// </example>
    public class DateTimeUtcConverter: System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        /// <summary>
        /// Reads and converts the JSON to type DateTime />.
        /// </summary>
        /// <param name="reader">The reader.</param>
        /// <param name="typeToConvert">The type to convert.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        /// <returns>The converted value.</returns>
        public override DateTime Read(ref Utf8JsonReader reader, [NotNull] Type typeToConvert, JsonSerializerOptions options)
        {
            Debug.Assert(typeToConvert == typeof(DateTime));
            return DateTime.Parse(reader.GetString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);
        }

        /// <summary>Writes a specified value as JSON.</summary>
        /// <param name="writer">The writer to write to.</param>
        /// <param name="value">The value to convert to JSON.</param>
        /// <param name="options">An object that specifies serialization options to use.</param>
        public override void Write([NotNull] Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToUniversalTime().ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ssZ"));
        }

        
    }

}