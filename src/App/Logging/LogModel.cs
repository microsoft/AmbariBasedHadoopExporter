// <copyright file="LogModel.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Logging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Newtonsoft.Json;
    using Serilog.Events;

    /// <summary>
    /// Simple log model that will be used to represend a log entry containing all relevant information.
    /// </summary>
    internal class LogModel
    {
        public LogModel(LogEvent logEvent, IEnumerable<string> ignoreProperties = null)
        {
            Message = logEvent.RenderMessage();
            Timestamp = logEvent.Timestamp.UtcDateTime;
            Level = logEvent.Level.ToString();
            LevelId = logEvent.Level;

            if (logEvent.Properties.Count > 0)
            {
                CustomFields = ignoreProperties == null ? logEvent.Properties : logEvent.Properties.Where(p => !ignoreProperties.Contains(p.Key)).ToDictionary(p => p.Key, p => p.Value);
            }

            if (logEvent.Exception != null)
            {
                Exception = new ExceptionLogModel
                {
                    Type = logEvent.Exception.GetType().ToString(),
                    Message = logEvent.Exception.Message,
                    StackTrace = logEvent.Exception.StackTrace,
                };
            }
        }

        [JsonProperty("LogTimestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("Level")]
        public string Level { get; set; }

        [JsonProperty("LevelId")]
        public LogEventLevel LevelId { get; set; }

        [JsonProperty("Message")]
        public string Message { get; set; }

        [JsonProperty("Exception")]
        public ExceptionLogModel Exception { get; set; }

        [JsonProperty("CustomFields")]
        public IReadOnlyDictionary<string, LogEventPropertyValue> CustomFields { get; set; }

        public class ExceptionLogModel
        {
            [JsonProperty("Type")]
            public string Type { get; set; }

            [JsonProperty("Message")]
            public string Message { get; set; }

            [JsonProperty("StackTrace")]
            public string StackTrace { get; set; }
        }
    }
}
