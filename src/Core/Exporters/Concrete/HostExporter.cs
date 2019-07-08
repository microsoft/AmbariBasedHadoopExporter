// <copyright file="HostExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Extensions;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    internal class HostExporter : BaseExporter
    {
        public readonly HostExporterConfiguration _hostConfiguration;

        public HostExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<HostExporterConfiguration> configuration,
            ILogger logger)
            : base(contentProvider, prometheusUtils, configuration.Value, typeof(ClusterHostComponent), logger)
        {
            _hostConfiguration = configuration.Value;
        }

        /// <inheritdoc/>
        protected override async Task ReportMetrics(object component)
        {
            await Task.Factory.StartNew(() =>
            {
                var clusterComponent = component as ClusterHostComponent;

                // Constructing labels
                var labels = new Dictionary<string, string>()
                {
                    { "ClusterName", clusterComponent.HostDetails.ClusterName },
                    { "Component", $"Hosts/{clusterComponent.HostDetails.HostName}" },
                };
                labels.TryAdd(_hostConfiguration.DefaultLabels);

                // Disk
                PrometheusUtils.ReportGauge(Collectors, "Disk_Free", clusterComponent.Metrics.Disk.Free, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", clusterComponent.Metrics.Disk.ReadBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", clusterComponent.Metrics.Disk.ReadCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", clusterComponent.Metrics.Disk.ReadTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_Disk", clusterComponent.Metrics.Disk.Total, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", clusterComponent.Metrics.Disk.WriteBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", clusterComponent.Metrics.Disk.WriteCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", clusterComponent.Metrics.Disk.WriteTime, labels);

                // Cpu
                PrometheusUtils.ReportGauge(Collectors, "HostCpu_Idle", clusterComponent.Metrics.HostCpu.Idle, labels);
                PrometheusUtils.ReportGauge(Collectors, "HostCpu_Nice", clusterComponent.Metrics.HostCpu.Nice, labels);
                PrometheusUtils.ReportGauge(Collectors, "HostCpu_Num", clusterComponent.Metrics.HostCpu.Num, labels);
                PrometheusUtils.ReportGauge(Collectors, "HostCpu_System", clusterComponent.Metrics.HostCpu.System, labels);
                PrometheusUtils.ReportGauge(Collectors, "HostCpu_User", clusterComponent.Metrics.HostCpu.User, labels);
                PrometheusUtils.ReportGauge(Collectors, "HostCpu_Wio", clusterComponent.Metrics.HostCpu.Wio, labels);

                // Memory
                PrometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", clusterComponent.Metrics.Memory.CachedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", clusterComponent.Metrics.Memory.FreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_Critical", clusterComponent.Metrics.Memory.SharedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", clusterComponent.Metrics.Memory.SwapFreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", clusterComponent.Metrics.Memory.TotalKb, labels);

                // Network
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesIn", clusterComponent.Metrics.Network.BytesIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesOut", clusterComponent.Metrics.Network.BytesOut, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsIn", clusterComponent.Metrics.Network.PktsIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsOut", clusterComponent.Metrics.Network.PktsOut, labels);

                // Processes
                PrometheusUtils.ReportGauge(Collectors, "Process_Run", clusterComponent.Metrics.Process.Run, labels);
                PrometheusUtils.ReportGauge(Collectors, "Process_Total", clusterComponent.Metrics.Process.Total, labels);
            });
        }
    }
}
