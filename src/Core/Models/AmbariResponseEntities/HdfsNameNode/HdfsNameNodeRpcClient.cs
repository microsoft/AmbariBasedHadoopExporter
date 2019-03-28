// <copyright file="HdfsNameNodeRpcClient.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Newtonsoft.Json;

    public class HdfsNameNodeRpcClient
    {
        [JsonProperty("NumOpenConnections")]
        public int NumOpenConnections { get; set; }

        [JsonProperty("ReceivedBytes")]
        public long ReceivedBytes { get; set; }

        [JsonProperty("ProcessingTimeAvgTime")]
        public double ProcessingTimeAvgTime { get; set; }

        [JsonProperty("QueueTimeAvgTime")]
        public double QueueTimeAvgTime { get; set; }

        [JsonProperty("SentBytes")]
        public int SentBytes { get; set; }
    }
}
