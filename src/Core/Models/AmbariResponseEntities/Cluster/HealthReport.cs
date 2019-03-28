// <copyright file="HealthReport.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using Newtonsoft.Json;

    public class HealthReport
    {
        [JsonProperty(PropertyName = "Host/stale_config")]
        public int HostsWithStaleConfig { get; set; }

        [JsonProperty(PropertyName = "Host/maintenance_state")]
        public int HostsWithMaintenanceFlag { get; set; }

        [JsonProperty(PropertyName = "Host/host_state/HEALTHY")]
        public int HostsStateHealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_state/UNHEALTHY")]
        public int HostsStateUnhealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_state/HEARTBEAT_LOST")]
        public int HeartbeatLost { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/HEALTHY")]
        public int HostsStatusHealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/UNHEALTHY")]
        public int HostsStatusUnhealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/UNKNOWN")]
        public int HostsStatusUnknown { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/ALERT")]
        public int HostsStatusAlert { get; set; }
    }
}
