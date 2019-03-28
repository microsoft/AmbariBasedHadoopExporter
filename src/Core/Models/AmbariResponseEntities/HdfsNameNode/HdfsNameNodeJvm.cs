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
        public int GcCountConcurrentMarkSweep { get; set; }

        [JsonProperty("GcTimeMillisConcurrentMarkSweep")]
        public int GcTimeMillisConcurrentMarkSweep { get; set; }

        [JsonProperty("gcCount")]
        public int GcCount { get; set; }

        [JsonProperty("gcTimeMillis")]
        public int GcTimeMillis { get; set; }

        [JsonProperty("logError")]
        public int LogError { get; set; }

        [JsonProperty("logFatal")]
        public int LogFatal { get; set; }

        [JsonProperty("logInfo")]
        public int LogInfo { get; set; }

        [JsonProperty("logWarn")]
        public int LogWarn { get; set; }

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
        public int ThreadsBlocked { get; set; }

        [JsonProperty("threadsNew")]
        public int ThreadsNew { get; set; }

        [JsonProperty("threadsRunnable")]
        public int ThreadsRunnable { get; set; }

        [JsonProperty("threadsTerminated")]
        public int ThreadsTerminated { get; set; }

        [JsonProperty("threadsTimedWaiting")]
        public int ThreadsTimedWaiting { get; set; }

        [JsonProperty("threadsWaiting")]
        public int ThreadsWaiting { get; set; }
    }
}
