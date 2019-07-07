// <copyright file="ClusterExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Exporters.Abstract;
    using Core.Extensions;
    using Core.Models.AmbariResponseEntities.Cluster;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Prometheus;

    /// <summary>
    /// The cluster component exporter, responsible exporting the cluster and its hosts metrics.
    /// </summary>
    internal class ClusterExporter : IExporter
    {
        private readonly IContentProvider _contentProvider;
        private readonly IPrometheusUtils _prometheusUtils;
        private readonly ClusterExporterConfiguration _exporterConfiguration;
        private readonly ILogger<ClusterExporter> _logger;

        public ClusterExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<ClusterExporterConfiguration> exporterConfiguration,
            ILogger<ClusterExporter> logger)
        {
            _contentProvider = contentProvider;
            _prometheusUtils = prometheusUtils;
            _exporterConfiguration = exporterConfiguration.Value;
            _logger = logger;
            Collectors = new ConcurrentDictionary<string, Collector>();
        }

        /// <inheritdoc/>
        public ConcurrentDictionary<string, Collector> Collectors { get; private set; }

        /// <inheritdoc/>
        public async Task ExportMetricsAsync()
        {
            var content = string.Empty;
            try
            {
                using (_logger.BeginScope(new Dictionary<string, object>() { { "Exporter", GetType().Name }, }))
                {
                    _logger.LogInformation($"{nameof(ExportMetricsAsync)} Started.");
                    var stopWatch = Stopwatch.StartNew();

                    content = await _contentProvider.GetResponseContentAsync(_exporterConfiguration.UriEndpoint);
                    var clusterComponent = JsonConvert.DeserializeObject<ClusterComponent>(content);

                    var hostsTasksList = ExportHostsMetricsAsync(clusterComponent.HostEntryList);

                    // Constructing labels
                    var labels = new Dictionary<string, string>()
                    {
                        { "ClusterName", clusterComponent.ClusterReport.ClusterName },
                        { "Component", "Cluster" },
                    };
                    labels.TryAdd(_exporterConfiguration.DefaultLabels);

                    // Health Report
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HeartbeatLost", clusterComponent.ClusterReport.HealthReport.HeartbeatLost, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStateHealthy", clusterComponent.ClusterReport.HealthReport.HostsStateHealthy, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStateHealthy", clusterComponent.ClusterReport.HealthReport.HostsStateUnhealthy, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusAlert", clusterComponent.ClusterReport.HealthReport.HostsStatusAlert, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusHealthy", clusterComponent.ClusterReport.HealthReport.HostsStatusHealthy, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusUnhealthy", clusterComponent.ClusterReport.HealthReport.HostsStatusUnhealthy, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusUnknown", clusterComponent.ClusterReport.HealthReport.HostsStatusUnknown, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsWithMaintenanceFlag", clusterComponent.ClusterReport.HealthReport.HostsWithMaintenanceFlag, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HealthReport_HostsWithStaleConfig", clusterComponent.ClusterReport.HealthReport.HostsWithStaleConfig, labels);

                    // Alerts Summary
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummary_Critical", clusterComponent.AlertsSummary.Critical, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummary_Maintenance", clusterComponent.AlertsSummary.Maintenance, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummary_Ok", clusterComponent.AlertsSummary.Ok, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummary_Unknown", clusterComponent.AlertsSummary.Unknown, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummary_Warning", clusterComponent.AlertsSummary.Warning, labels);

                    // Alerts Summary Hosts
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Critical", clusterComponent.AlertsSummaryHosts.Critical, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Ok", clusterComponent.AlertsSummaryHosts.Ok, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Unknown", clusterComponent.AlertsSummaryHosts.Unknown, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Warning", clusterComponent.AlertsSummaryHosts.Warning, labels);

                    await Task.WhenAll(hostsTasksList);

                    // Tracing
                    stopWatch.Stop();
                    _logger.LogInformation($"Runtime: {stopWatch.Elapsed}.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(ClusterExporter)}.{nameof(ExportMetricsAsync)}: Failed to export metrics. Content: {content}");
                throw;
            }
        }

        /// <summary>
        /// Exporting metrics for all cluster nodes in the cluster.
        /// </summary>
        /// <param name="hosts">IEnumerable of HostEntry.</param>
        /// <returns>List of tasks, each task exporting metrics of a specific host.</returns>
        internal List<Task> ExportHostsMetricsAsync(IEnumerable<HostEntry> hosts)
        {
            try
            {
                var tempTaskList = new List<Task>();
                foreach (var host in hosts)
                {
                    tempTaskList.Add(ExportHostMetricsAsync(host.Host.HostName));
                }

                return tempTaskList;
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"{nameof(ClusterExporter)}.{nameof(ExportHostsMetricsAsync)}: Failed to export metrics.");
                throw;
            }
        }

        /// <summary>
        /// Exporting the specified host metrics.
        /// </summary>
        /// <param name="hostName">Host name.</param>
        /// <returns>A task that exports the current host metrics.</returns>
        internal async Task ExportHostMetricsAsync(string hostName)
        {
            if (hostName == null || hostName.Equals(string.Empty))
            {
                throw new ArgumentException($"Invalid argument was passed." +
                                            $"HostName - cannot be null/empty: {hostName}.");
            }

            var content = string.Empty;
            using (_logger.BeginScope(new Dictionary<string, object> { { "HostName", hostName }, }))
            {
                try
                {
                    _logger.LogInformation($"{nameof(ExportHostMetricsAsync)} Started.");
                    var stopWatch = Stopwatch.StartNew();

                    content = await _contentProvider.GetResponseContentAsync($"{_exporterConfiguration.HostsEndpoint}/{hostName}");
                    var component = JsonConvert.DeserializeObject<ClusterHostComponent>(content);

                    // Constructing labels
                    var labels = new Dictionary<string, string>()
                        {
                            { "ClusterName", component.HostDetails.ClusterName },
                            { "Component", $"Hosts/{component.HostDetails.HostName}" },
                        };
                    labels.TryAdd(_exporterConfiguration.DefaultLabels);

                    // Disk
                    _prometheusUtils.ReportGauge(Collectors, "Disk_Free", component.Metrics.Disk.Free, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", component.Metrics.Disk.ReadBytes, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", component.Metrics.Disk.ReadCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", component.Metrics.Disk.ReadTime, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_Disk", component.Metrics.Disk.Total, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", component.Metrics.Disk.WriteBytes, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", component.Metrics.Disk.WriteCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", component.Metrics.Disk.WriteTime, labels);

                    // Cpu
                    _prometheusUtils.ReportGauge(Collectors, "HostCpu_Idle", component.Metrics.HostCpu.Idle, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HostCpu_Nice", component.Metrics.HostCpu.Nice, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HostCpu_Num", component.Metrics.HostCpu.Num, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HostCpu_System", component.Metrics.HostCpu.System, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HostCpu_User", component.Metrics.HostCpu.User, labels);
                    _prometheusUtils.ReportGauge(Collectors, "HostCpu_Wio", component.Metrics.HostCpu.Wio, labels);

                    // Memory
                    _prometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", component.Metrics.Memory.CachedKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", component.Metrics.Memory.FreeKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_Critical", component.Metrics.Memory.SharedKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", component.Metrics.Memory.SwapFreeKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", component.Metrics.Memory.TotalKb, labels);

                    // Network
                    _prometheusUtils.ReportGauge(Collectors, "Network_BytesIn", component.Metrics.Network.BytesIn, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Network_BytesOut", component.Metrics.Network.BytesOut, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Network_PktsIn", component.Metrics.Network.PktsIn, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Network_PktsOut", component.Metrics.Network.PktsOut, labels);

                    // Processes
                    _prometheusUtils.ReportGauge(Collectors, "Process_Run", component.Metrics.Process.Run, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Process_Total", component.Metrics.Process.Total, labels);

                    // Tracing
                    stopWatch.Stop();
                    _logger.LogInformation($"Runtime: {stopWatch.Elapsed}.");
                }
                catch (Exception e)
                {
                    _logger.LogError(e, $"{nameof(ClusterExporter)}.{nameof(ExportMetricsAsync)}: Failed to export metrics for host {hostName}. Content: {content}");
                    throw;
                }
            }
        }
    }
}
