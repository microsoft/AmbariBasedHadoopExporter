// <copyright file="Queue.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    public class Queue
    {
        [JsonExtensionData]
        private readonly IDictionary<string, JToken> _additionalData;

        public Queue()
        {
            Name = "root"; // default is root
            _additionalData = new Dictionary<string, JToken>();
        }

        [JsonProperty(PropertyName = "AMResourceLimitMB")]
        public long AMResourceLimitMB { get; set; }

        [JsonProperty(PropertyName = "AMResourceLimitVCores")]
        public long AMResourceLimitVCores { get; set; }

        [JsonProperty(PropertyName = "ActiveApplications")]
        public long ActiveApplications { get; set; }

        [JsonProperty(PropertyName = "ActiveUsers")]
        public long ActiveUsers { get; set; }

        [JsonProperty(PropertyName = "AggregateContainersAllocated")]
        public long AggregateContainersAllocated { get; set; }

        [JsonProperty(PropertyName = "AggregateContainersReleased")]
        public long AggregateContainersReleased { get; set; }

        [JsonProperty(PropertyName = "AllocatedContainers")]
        public long AllocatedContainers { get; set; }

        [JsonProperty(PropertyName = "AllocatedMB")]
        public long AllocatedMB { get; set; }

        [JsonProperty(PropertyName = "AllocatedVCores")]
        public long AllocatedVCores { get; set; }

        [JsonProperty(PropertyName = "AppAttemptFirstContainerAllocationDelayAvgTime")]
        public double AppAttemptFirstContainerAllocationDelayAvgTime { get; set; }

        [JsonProperty(PropertyName = "AppAttemptFirstContainerAllocationDelayNumOps")]
        public long AppAttemptFirstContainerAllocationDelayNumOps { get; set; }

        [JsonProperty(PropertyName = "AppsCompleted")]
        public long AppsCompleted { get; set; }

        [JsonProperty(PropertyName = "AppsFailed")]
        public long AppsFailed { get; set; }

        [JsonProperty(PropertyName = "AppsKilled")]
        public long AppsKilled { get; set; }

        [JsonProperty(PropertyName = "AppsPending")]
        public long AppsPending { get; set; }

        [JsonProperty(PropertyName = "AppsRunning")]
        public long AppsRunning { get; set; }

        [JsonProperty(PropertyName = "AppsSubmitted")]
        public long AppsSubmitted { get; set; }

        [JsonProperty(PropertyName = "AvailableMB")]
        public long AvailableMB { get; set; }

        [JsonProperty(PropertyName = "AvailableVCores")]
        public long AvailableVCores { get; set; }

        [JsonProperty(PropertyName = "PendingContainers")]
        public long PendingContainers { get; set; }

        [JsonProperty(PropertyName = "PendingMB")]
        public long PendingMB { get; set; }

        [JsonProperty(PropertyName = "PendingVCores")]
        public long PendingVCores { get; set; }

        [JsonProperty(PropertyName = "ReservedContainers")]
        public long ReservedContainers { get; set; }

        [JsonProperty(PropertyName = "ReservedMB")]
        public long ReservedMB { get; set; }

        [JsonProperty(PropertyName = "ReservedVCores")]
        public long ReservedVCores { get; set; }

        [JsonProperty(PropertyName = "UsedAMResourceMB")]
        public long UsedAMResourceMB { get; set; }

        [JsonProperty(PropertyName = "UsedAMResourceVCores")]
        public long UsedAMResourceVCores { get; set; }

        [JsonProperty(PropertyName = "running_0")]
        public long Running0 { get; set; }

        [JsonProperty(PropertyName = "running_1440")]
        public long Running1440 { get; set; }

        [JsonProperty(PropertyName = "running_300")]
        public long Running300 { get; set; }

        [JsonProperty(PropertyName = "running_60")]
        public long Running60 { get; set; }

        public string Name { get; private set; }

        public IEnumerable<Queue> GetChildrenQueuesIEnumerable()
        {
            var queues = new List<Queue>();
            foreach (var item in _additionalData)
            {
                var queue = item.Value.ToObject<Queue>();
                queue.Name = item.Key;
                queues.Add(queue);
            }

            return queues;
        }
    }
}
