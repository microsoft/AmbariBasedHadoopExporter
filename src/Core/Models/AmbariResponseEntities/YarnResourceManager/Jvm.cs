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
        public int HeapMemoryMax { get; set; }

        [JsonProperty(PropertyName = "HeapMemoryUsed")]
        public int HeapMemoryUsed { get; set; }

        [JsonProperty(PropertyName = "NonHeapMemoryMax")]
        public int NonHeapMemoryMax { get; set; }

        [JsonProperty(PropertyName = "NonHeapMemoryUsed")]
        public int NonHeapMemoryUsed { get; set; }

        [JsonProperty(PropertyName = "gcCount")]
        public int GcCount { get; set; }

        [JsonProperty(PropertyName = "gcTimeMillis")]
        public int GcTimeMillis { get; set; }

        [JsonProperty(PropertyName = "logError")]
        public int LogError { get; set; }

        [JsonProperty(PropertyName = "logFatal")]
        public int LogFatal { get; set; }

        [JsonProperty(PropertyName = "logInfo")]
        public int LogInfo { get; set; }

        [JsonProperty(PropertyName = "logWarn")]
        public int LogWarn { get; set; }

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
        public int ThreadsBlocked { get; set; }

        [JsonProperty(PropertyName = "threadsNew")]
        public int ThreadsNew { get; set; }

        [JsonProperty(PropertyName = "threadsRunnable")]
        public int ThreadsRunnable { get; set; }

        [JsonProperty(PropertyName = "threadsTerminated")]
        public int ThreadsTerminated { get; set; }

        [JsonProperty(PropertyName = "threadsTimedWaiting")]
        public int ThreadsTimedWaiting { get; set; }

        [JsonProperty(PropertyName = "threadsWaiting")]
        public int ThreadsWaiting { get; set; }
    }
}
