// <copyright file="HdfsNameNodeJvm.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Newtonsoft.Json;

    public class HdfsNameNodeJvm
    {
        [JsonProperty("GcCountConcurrentMarkSweep")]
        public long GcCountConcurrentMarkSweep { get; set; }

        [JsonProperty("GcTimeMillisConcurrentMarkSweep")]
        public long GcTimeMillisConcurrentMarkSweep { get; set; }

        [JsonProperty("gcCount")]
        public long GcCount { get; set; }

        [JsonProperty("gcTimeMillis")]
        public long GcTimeMillis { get; set; }

        [JsonProperty("logError")]
        public long LogError { get; set; }

        [JsonProperty("logFatal")]
        public long LogFatal { get; set; }

        [JsonProperty("logInfo")]
        public long LogInfo { get; set; }

        [JsonProperty("logWarn")]
        public long LogWarn { get; set; }

        [JsonProperty("memHeapCommittedM")]
        public double MemHeapCommittedM { get; set; }

        [JsonProperty("memHeapUsedM")]
        public double MemHeapUsedM { get; set; }

        [JsonProperty("memMaxM")]
        public double MemMaxM { get; set; }

        [JsonProperty("memNonHeapCommittedM")]

        public double MemNonHeapCommittedM { get; set; }

        [JsonProperty("memNonHeapUsedM")]

        public double MemNonHeapUsedM { get; set; }

        [JsonProperty("threadsBlocked")]
        public long ThreadsBlocked { get; set; }

        [JsonProperty("threadsNew")]
        public long ThreadsNew { get; set; }

        [JsonProperty("threadsRunnable")]
        public long ThreadsRunnable { get; set; }

        [JsonProperty("threadsTerminated")]
        public long ThreadsTerminated { get; set; }

        [JsonProperty("threadsTimedWaiting")]
        public long ThreadsTimedWaiting { get; set; }

        [JsonProperty("threadsWaiting")]
        public long ThreadsWaiting { get; set; }
    }
}
