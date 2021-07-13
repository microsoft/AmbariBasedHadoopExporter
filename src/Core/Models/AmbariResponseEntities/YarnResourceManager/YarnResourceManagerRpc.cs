// <copyright file="YarnResourceManagerRpc.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class YarnResourceManagerRpc
    {
        [JsonProperty(PropertyName = "NumOpenConnections")]
        public long NumOpenConnections { get; set; }

        [JsonProperty(PropertyName = "ReceivedBytes")]
        public long ReceivedBytes { get; set; }

        [JsonProperty(PropertyName = "RpcProcessingTime_avg_time")]
        public double RpcProcessingTime_avg_time { get; set; }

        [JsonProperty(PropertyName = "RpcProcessingTime_num_ops")]
        public long RpcProcessingTime_num_ops { get; set; }

        [JsonProperty(PropertyName = "RpcQueueTime_avg_time")]
        public double RpcQueueTime_avg_time { get; set; }

        [JsonProperty(PropertyName = "RpcQueueTime_num_ops")]
        public long RpcQueueTime_num_ops { get; set; }

        [JsonProperty(PropertyName = "SentBytes")]
        public long SentBytes { get; set; }

        [JsonProperty(PropertyName = "callQueueLen")]
        public long CallQueueLen { get; set; }

        [JsonProperty(PropertyName = "rpcAuthenticationFailures")]
        public long RpcAuthenticationFailures { get; set; }

        [JsonProperty(PropertyName = "rpcAuthenticationSuccesses")]
        public long RpcAuthenticationSuccesses { get; set; }

        [JsonProperty(PropertyName = "rpcAuthorizationFailures")]
        public long RpcAuthorizationFailures { get; set; }

        [JsonProperty(PropertyName = "rpcAuthorizationSuccesses")]
        public long RpcAuthorizationSuccesses { get; set; }
    }
}
