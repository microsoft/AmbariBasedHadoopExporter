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
        public int AMResourceLimitMB { get; set; }

        [JsonProperty(PropertyName = "AMResourceLimitVCores")]
        public int AMResourceLimitVCores { get; set; }

        [JsonProperty(PropertyName = "ActiveApplications")]
        public int ActiveApplications { get; set; }

        [JsonProperty(PropertyName = "ActiveUsers")]
        public int ActiveUsers { get; set; }

        [JsonProperty(PropertyName = "AggregateContainersAllocated")]
        public int AggregateContainersAllocated { get; set; }

        [JsonProperty(PropertyName = "AggregateContainersReleased")]
        public int AggregateContainersReleased { get; set; }

        [JsonProperty(PropertyName = "AllocatedContainers")]
        public int AllocatedContainers { get; set; }

        [JsonProperty(PropertyName = "AllocatedMB")]
        public int AllocatedMB { get; set; }

        [JsonProperty(PropertyName = "AllocatedVCores")]
        public int AllocatedVCores { get; set; }

        [JsonProperty(PropertyName = "AppAttemptFirstContainerAllocationDelayAvgTime")]
        public double AppAttemptFirstContainerAllocationDelayAvgTime { get; set; }

        [JsonProperty(PropertyName = "AppAttemptFirstContainerAllocationDelayNumOps")]
        public int AppAttemptFirstContainerAllocationDelayNumOps { get; set; }

        [JsonProperty(PropertyName = "AppsCompleted")]
        public int AppsCompleted { get; set; }

        [JsonProperty(PropertyName = "AppsFailed")]
        public int AppsFailed { get; set; }

        [JsonProperty(PropertyName = "AppsKilled")]
        public int AppsKilled { get; set; }

        [JsonProperty(PropertyName = "AppsPending")]
        public int AppsPending { get; set; }

        [JsonProperty(PropertyName = "AppsRunning")]
        public int AppsRunning { get; set; }

        [JsonProperty(PropertyName = "AppsSubmitted")]
        public int AppsSubmitted { get; set; }

        [JsonProperty(PropertyName = "AvailableMB")]
        public int AvailableMB { get; set; }

        [JsonProperty(PropertyName = "AvailableVCores")]
        public int AvailableVCores { get; set; }

        [JsonProperty(PropertyName = "PendingContainers")]
        public int PendingContainers { get; set; }

        [JsonProperty(PropertyName = "PendingMB")]
        public int PendingMB { get; set; }

        [JsonProperty(PropertyName = "PendingVCores")]
        public int PendingVCores { get; set; }

        [JsonProperty(PropertyName = "ReservedContainers")]
        public int ReservedContainers { get; set; }

        [JsonProperty(PropertyName = "ReservedMB")]
        public int ReservedMB { get; set; }

        [JsonProperty(PropertyName = "ReservedVCores")]
        public int ReservedVCores { get; set; }

        [JsonProperty(PropertyName = "UsedAMResourceMB")]
        public int UsedAMResourceMB { get; set; }

        [JsonProperty(PropertyName = "UsedAMResourceVCores")]
        public int UsedAMResourceVCores { get; set; }

        [JsonProperty(PropertyName = "running_0")]
        public int Running0 { get; set; }

        [JsonProperty(PropertyName = "running_1440")]
        public int Running1440 { get; set; }

        [JsonProperty(PropertyName = "running_300")]
        public int Running300 { get; set; }

        [JsonProperty(PropertyName = "running_60")]
        public int Running60 { get; set; }

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
