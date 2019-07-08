// <copyright file="YarnNodeManagerExporter.cs" company="Microsoft">
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
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Prometheus;

    /// <summary>
    /// Responsible for exporting YARN NodeManager metrics.
    /// </summary>
    internal class YarnNodeManagerExporter : BaseExporter
    {
        private readonly YarnNodeManagerExporterConfiguration _exporterConfiguration;

        public YarnNodeManagerExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<YarnNodeManagerExporterConfiguration> exporterConfiguration,
            ILogger<YarnNodeManagerExporter> logger)
            : base(contentProvider, prometheusUtils, exporterConfiguration.Value, typeof(NodeManagerComponent), logger)
        {
            _exporterConfiguration = exporterConfiguration.Value;
        }

        /// <inheritdoc/>
        protected override async Task ReportMetrics(object component)
        {
            await Task.Factory.StartNew(() =>
            {
                var nodeManagerComponent = (NodeManagerComponent)component;

                // Constructing labels
                var labels = new Dictionary<string, string>()
                {
                    { "ClusterName", nodeManagerComponent.Info.ClusterName },
                    { "Component", nodeManagerComponent.Info.ComponentName },
                };
                labels.TryAdd(_exporterConfiguration.DefaultLabels);

                // General info
                PrometheusUtils.ReportGauge(Collectors, "Info_StartTime", nodeManagerComponent.Info.StartTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_StartedCount", nodeManagerComponent.Info.StartedCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_TotalCount", nodeManagerComponent.Info.TotalCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", nodeManagerComponent.Info.UnknownCount, labels);

                // Yarn related
                PrometheusUtils.ReportGauge(Collectors, "AllocatedContainers", nodeManagerComponent.Metrics.YarnBase.AllocatedContainers, labels);
                PrometheusUtils.ReportGauge(Collectors, "AllocatedGB", nodeManagerComponent.Metrics.YarnBase.AllocatedGB, labels);
                PrometheusUtils.ReportGauge(Collectors, "AllocatedVCores", nodeManagerComponent.Metrics.YarnBase.AllocatedVCores, labels);
                PrometheusUtils.ReportGauge(Collectors, "ContainersCompleted", nodeManagerComponent.Metrics.YarnBase.ContainersCompleted, labels);
                PrometheusUtils.ReportGauge(Collectors, "ContainersFailed", nodeManagerComponent.Metrics.YarnBase.ContainersFailed, labels);
                PrometheusUtils.ReportGauge(Collectors, "ContainersIniting", nodeManagerComponent.Metrics.YarnBase.ContainersIniting, labels);
                PrometheusUtils.ReportGauge(Collectors, "ContainersKilled", nodeManagerComponent.Metrics.YarnBase.ContainersKilled, labels);
                PrometheusUtils.ReportGauge(Collectors, "ContainersLaunched", nodeManagerComponent.Metrics.YarnBase.ContainersLaunched, labels);
                PrometheusUtils.ReportGauge(Collectors, "ContainersRunning", nodeManagerComponent.Metrics.YarnBase.ContainersRunning, labels);

                // Cpu
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Idle", nodeManagerComponent.Metrics.Cpu.Idle, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Nice", nodeManagerComponent.Metrics.Cpu.Nice, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_System", nodeManagerComponent.Metrics.Cpu.System, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_User", nodeManagerComponent.Metrics.Cpu.User, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Wio", nodeManagerComponent.Metrics.Cpu.Wio, labels);

                // Disk
                PrometheusUtils.ReportGauge(Collectors, "Disk_Free", nodeManagerComponent.Metrics.Disk.Free, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", nodeManagerComponent.Metrics.Disk.ReadBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", nodeManagerComponent.Metrics.Disk.ReadCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", nodeManagerComponent.Metrics.Disk.ReadTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_Total", nodeManagerComponent.Metrics.Disk.Total, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", nodeManagerComponent.Metrics.Disk.WriteBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", nodeManagerComponent.Metrics.Disk.WriteCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", nodeManagerComponent.Metrics.Disk.WriteTime, labels);

                // Memory
                PrometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", nodeManagerComponent.Metrics.Memory.CachedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", nodeManagerComponent.Metrics.Memory.FreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SharedKb", nodeManagerComponent.Metrics.Memory.SharedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", nodeManagerComponent.Metrics.Memory.SwapFreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", nodeManagerComponent.Metrics.Memory.TotalKb, labels);

                // Network
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesIn", nodeManagerComponent.Metrics.Network.BytesIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesOut", nodeManagerComponent.Metrics.Network.BytesOut, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsIn", nodeManagerComponent.Metrics.Network.PktsIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsOut", nodeManagerComponent.Metrics.Network.PktsOut, labels);

                // Process
                PrometheusUtils.ReportGauge(Collectors, "Process_Run", nodeManagerComponent.Metrics.Process.Run, labels);
                PrometheusUtils.ReportGauge(Collectors, "Process_Total", nodeManagerComponent.Metrics.Process.Total, labels);
            });
        }
    }
}
