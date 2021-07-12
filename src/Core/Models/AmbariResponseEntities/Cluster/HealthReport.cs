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
        public long HostsWithStaleConfig { get; set; }

        [JsonProperty(PropertyName = "Host/maintenance_state")]
        public long HostsWithMaintenanceFlag { get; set; }

        [JsonProperty(PropertyName = "Host/host_state/HEALTHY")]
        public long HostsStateHealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_state/UNHEALTHY")]
        public long HostsStateUnhealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_state/HEARTBEAT_LOST")]
        public long HeartbeatLost { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/HEALTHY")]
        public long HostsStatusHealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/UNHEALTHY")]
        public long HostsStatusUnhealthy { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/UNKNOWN")]
        public long HostsStatusUnknown { get; set; }

        [JsonProperty(PropertyName = "Host/host_status/ALERT")]
        public long HostsStatusAlert { get; set; }
    }
}
