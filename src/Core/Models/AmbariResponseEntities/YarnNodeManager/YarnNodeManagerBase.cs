// <copyright file="YarnNodeManagerBase.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnNodeManager
{
    using Newtonsoft.Json;

    public class YarnNodeManagerBase
    {
        [JsonProperty(PropertyName = "AllocatedContainers")]
        public double AllocatedContainers { get; set; }

        [JsonProperty(PropertyName = "AllocatedGB")]
        public double AllocatedGB { get; set; }

        [JsonProperty(PropertyName = "AllocatedVCores")]
        public double AllocatedVCores { get; set; }

        [JsonProperty(PropertyName = "ContainersCompleted")]
        public double ContainersCompleted { get; set; }

        [JsonProperty(PropertyName = "ContainersFailed")]
        public double ContainersFailed { get; set; }

        [JsonProperty(PropertyName = "ContainersIniting")]
        public double ContainersIniting { get; set; }

        [JsonProperty(PropertyName = "ContainersKilled")]
        public double ContainersKilled { get; set; }

        [JsonProperty(PropertyName = "ContainersLaunched")]
        public double ContainersLaunched { get; set; }

        [JsonProperty(PropertyName = "ContainersRunning")]
        public double ContainersRunning { get; set; }
    }
}
