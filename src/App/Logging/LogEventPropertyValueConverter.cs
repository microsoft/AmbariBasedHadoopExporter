// <copyright file="LogEventPropertyValueConverter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Logging
{
    using System;
    using System.IO;
    using Newtonsoft.Json;
    using Serilog.Events;
    using Serilog.Formatting.Json;

    /// <summary>
    /// Converter used to take a Serilog event and write it as a Json object.
    /// </summary>
    internal class LogEventPropertyValueConverter : JsonConverter<LogEventPropertyValue>
    {
        private readonly JsonValueFormatter _jsonValueFormatter = new JsonValueFormatter();

        public override void WriteJson(JsonWriter writer, LogEventPropertyValue value, JsonSerializer serializer)
        {
            // Prevent JsonSerializer from escaping the output (it is already in JSON format)
            using (var stringWriter = new StringWriter())
            {
                _jsonValueFormatter.Format(value, stringWriter);
                writer.WriteRawValue(stringWriter.ToString());
            }
        }

        public override LogEventPropertyValue ReadJson(
            JsonReader reader,
            Type objectType,
            LogEventPropertyValue existingValue,
            bool hasExistingValue,
            JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
