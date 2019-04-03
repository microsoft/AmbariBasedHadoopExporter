// <copyright file="LogFormatter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Logging
{
    using System.IO;
    using Newtonsoft.Json;
    using Serilog.Events;
    using Serilog.Formatting;
    using Serilog.Formatting.Json;

    public class LogFormatter : ITextFormatter
    {
        protected readonly JsonSerializer _serializer;

        public LogFormatter(JsonValueFormatter valueFormatter = null)
        {
            _serializer = new JsonSerializer
            {
                NullValueHandling = NullValueHandling.Ignore,
            };

            _serializer.Converters.Add(new LogEventPropertyValueConverter());
        }

        public void Format(LogEvent logEvent, TextWriter output)
        {
            var log = new LogModel(logEvent);
            _serializer.Serialize(output, log);
            output.WriteLine();
        }
    }
}
