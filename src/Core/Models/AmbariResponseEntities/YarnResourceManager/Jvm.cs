// <copyright file="Jvm.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class Jvm
    {
        [JsonProperty(PropertyName = "HeapMemoryMax")]
        public long HeapMemoryMax { get; set; }

        [JsonProperty(PropertyName = "HeapMemoryUsed")]
        public long HeapMemoryUsed { get; set; }

        [JsonProperty(PropertyName = "NonHeapMemoryMax")]
        public long NonHeapMemoryMax { get; set; }

        [JsonProperty(PropertyName = "NonHeapMemoryUsed")]
        public long NonHeapMemoryUsed { get; set; }

        [JsonProperty(PropertyName = "gcCount")]
        public long GcCount { get; set; }

        [JsonProperty(PropertyName = "gcTimeMillis")]
        public long GcTimeMillis { get; set; }

        [JsonProperty(PropertyName = "logError")]
        public long LogError { get; set; }

        [JsonProperty(PropertyName = "logFatal")]
        public long LogFatal { get; set; }

        [JsonProperty(PropertyName = "logInfo")]
        public long LogInfo { get; set; }

        [JsonProperty(PropertyName = "logWarn")]
        public long LogWarn { get; set; }

        [JsonProperty(PropertyName = "memHeapCommittedM")]
        public double MemHeapCommittedM { get; set; }

        [JsonProperty(PropertyName = "memHeapUsedM")]
        public double MemHeapUsedMB { get; set; }

        [JsonProperty(PropertyName = "memMaxM")]
        public double MemMaxMB { get; set; }

        [JsonProperty(PropertyName = "memNonHeapCommittedM")]
        public double MemNonHeapCommittedMB { get; set; }

        [JsonProperty(PropertyName = "memNonHeapUsedM")]
        public double MemNonHeapUsedMB { get; set; }

        [JsonProperty(PropertyName = "threadsBlocked")]
        public long ThreadsBlocked { get; set; }

        [JsonProperty(PropertyName = "threadsNew")]
        public long ThreadsNew { get; set; }

        [JsonProperty(PropertyName = "threadsRunnable")]
        public long ThreadsRunnable { get; set; }

        [JsonProperty(PropertyName = "threadsTerminated")]
        public long ThreadsTerminated { get; set; }

        [JsonProperty(PropertyName = "threadsTimedWaiting")]
        public long ThreadsTimedWaiting { get; set; }

        [JsonProperty(PropertyName = "threadsWaiting")]
        public long ThreadsWaiting { get; set; }
    }
}
