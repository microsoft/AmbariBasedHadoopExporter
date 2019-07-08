// <copyright file="ClusterExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Extensions;
    using Core.Models.AmbariResponseEntities.Cluster;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// The cluster component exporter, responsible exporting the cluster and its hosts metrics.
    /// </summary>
    internal class ClusterExporter : BaseExporter
    {
        internal readonly HostExporter _hostExporter;
        private readonly ClusterExporterConfiguration _clusterConfiguration;

        public ClusterExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<ClusterExporterConfiguration> exporterConfiguration,
            IOptions<HostExporterConfiguration> hostExporterConfiguration,
            ILogger<ClusterExporter> logger)
            : base(contentProvider, prometheusUtils, exporterConfiguration.Value, typeof(ClusterComponent), logger)
        {
            _clusterConfiguration = exporterConfiguration.Value;
            _hostExporter = new HostExporter(contentProvider, prometheusUtils, hostExporterConfiguration, logger);
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
                    tempTaskList.Add(_hostExporter.ExportMetricsAsync(host.Host.HostName));
                }

                return tempTaskList;
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"{nameof(ClusterExporter)}.{nameof(ExportHostsMetricsAsync)}: Failed to export metrics.");
                throw;
            }
        }

        /// <inheritdoc/>
        protected override async Task ReportMetrics(object component)
        {
            var clusterComponent = (ClusterComponent)component;
            var hostsTasksList = ExportHostsMetricsAsync(clusterComponent.HostEntryList);

            // Constructing labels
            var labels = new Dictionary<string, string>()
            {
                { "ClusterName", clusterComponent.ClusterReport.ClusterName },
                { "Component", "Cluster" },
            };
            labels.TryAdd(_clusterConfiguration.DefaultLabels);

            // Health Report
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HeartbeatLost", clusterComponent.ClusterReport.HealthReport.HeartbeatLost, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStateHealthy", clusterComponent.ClusterReport.HealthReport.HostsStateHealthy, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStateHealthy", clusterComponent.ClusterReport.HealthReport.HostsStateUnhealthy, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusAlert", clusterComponent.ClusterReport.HealthReport.HostsStatusAlert, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusHealthy", clusterComponent.ClusterReport.HealthReport.HostsStatusHealthy, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusUnhealthy", clusterComponent.ClusterReport.HealthReport.HostsStatusUnhealthy, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsStatusUnknown", clusterComponent.ClusterReport.HealthReport.HostsStatusUnknown, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsWithMaintenanceFlag", clusterComponent.ClusterReport.HealthReport.HostsWithMaintenanceFlag, labels);
            PrometheusUtils.ReportGauge(Collectors, "HealthReport_HostsWithStaleConfig", clusterComponent.ClusterReport.HealthReport.HostsWithStaleConfig, labels);

            // Alerts Summary
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummary_Critical", clusterComponent.AlertsSummary.Critical, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummary_Maintenance", clusterComponent.AlertsSummary.Maintenance, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummary_Ok", clusterComponent.AlertsSummary.Ok, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummary_Unknown", clusterComponent.AlertsSummary.Unknown, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummary_Warning", clusterComponent.AlertsSummary.Warning, labels);

            // Alerts Summary Hosts
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Critical", clusterComponent.AlertsSummaryHosts.Critical, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Ok", clusterComponent.AlertsSummaryHosts.Ok, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Unknown", clusterComponent.AlertsSummaryHosts.Unknown, labels);
            PrometheusUtils.ReportGauge(Collectors, "AlertsSummaryHosts_Warning", clusterComponent.AlertsSummaryHosts.Warning, labels);

            // Waiting for hosts
            await Task.WhenAll(hostsTasksList);
        }
    }
}
