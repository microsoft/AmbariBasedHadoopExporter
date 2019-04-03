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
    using System.Threading;
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
    internal class YarnNodeManagerExporter : IExporter
    {
        private readonly IContentProvider _contentProvider;
        private readonly IPrometheusUtils _prometheusUtils;
        private readonly YarnNodeManagerExporterConfiguration _exporterConfiguration;
        private readonly ILogger<YarnNodeManagerExporter> _logger;

        public YarnNodeManagerExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<YarnNodeManagerExporterConfiguration> exporterConfiguration,
            ILogger<YarnNodeManagerExporter> logger)
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
            try
            {
                using (_logger.BeginScope(new Dictionary<string, object>() { { "Exporter", GetType().Name }, }))
                {
                    _logger.LogInformation($"{nameof(ExportMetricsAsync)} Started.");
                    var stopWatch = Stopwatch.StartNew();

                    var content = await _contentProvider.GetResponseContentAsync(_exporterConfiguration.UriEndpoint);
                    var component = JsonConvert.DeserializeObject<NodeManagerComponent>(content);

                    // Constructing labels
                    var labels = new Dictionary<string, string>()
                        {
                            { "ClusterName", component.Info.ClusterName },
                            { "Component", component.Info.ComponentName },
                        };
                    labels.TryAdd(_exporterConfiguration.DefaultLabels);

                    // General info
                    _prometheusUtils.ReportGauge(Collectors, "Info_StartTime", component.Info.StartTime, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Info_StartedCount", component.Info.StartedCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Info_TotalCount", component.Info.TotalCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", component.Info.UnknownCount, labels);

                    // Yarn related
                    _prometheusUtils.ReportGauge(Collectors, "AllocatedContainers", component.Metrics.YarnBase.AllocatedContainers, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AllocatedGB", component.Metrics.YarnBase.AllocatedGB, labels);
                    _prometheusUtils.ReportGauge(Collectors, "AllocatedVCores", component.Metrics.YarnBase.AllocatedVCores, labels);
                    _prometheusUtils.ReportGauge(Collectors, "ContainersCompleted", component.Metrics.YarnBase.ContainersCompleted, labels);
                    _prometheusUtils.ReportGauge(Collectors, "ContainersFailed", component.Metrics.YarnBase.ContainersFailed, labels);
                    _prometheusUtils.ReportGauge(Collectors, "ContainersIniting", component.Metrics.YarnBase.ContainersIniting, labels);
                    _prometheusUtils.ReportGauge(Collectors, "ContainersKilled", component.Metrics.YarnBase.ContainersKilled, labels);
                    _prometheusUtils.ReportGauge(Collectors, "ContainersLaunched", component.Metrics.YarnBase.ContainersLaunched, labels);
                    _prometheusUtils.ReportGauge(Collectors, "ContainersRunning", component.Metrics.YarnBase.ContainersRunning, labels);

                    // Cpu
                    _prometheusUtils.ReportGauge(Collectors, "Cpu_Idle", component.Metrics.Cpu.Idle, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Cpu_Nice", component.Metrics.Cpu.Nice, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Cpu_System", component.Metrics.Cpu.System, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Cpu_User", component.Metrics.Cpu.User, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Cpu_Wio", component.Metrics.Cpu.Wio, labels);

                    // Disk
                    _prometheusUtils.ReportGauge(Collectors, "Disk_Free", component.Metrics.Disk.Free, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", component.Metrics.Disk.ReadBytes, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", component.Metrics.Disk.ReadCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", component.Metrics.Disk.ReadTime, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_Total", component.Metrics.Disk.Total, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", component.Metrics.Disk.WriteBytes, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", component.Metrics.Disk.WriteCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", component.Metrics.Disk.WriteTime, labels);

                    // Memory
                    _prometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", component.Metrics.Memory.CachedKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", component.Metrics.Memory.FreeKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_SharedKb", component.Metrics.Memory.SharedKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", component.Metrics.Memory.SwapFreeKb, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", component.Metrics.Memory.TotalKb, labels);

                    // Network
                    _prometheusUtils.ReportGauge(Collectors, "Network_BytesIn", component.Metrics.Network.BytesIn, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Network_BytesOut", component.Metrics.Network.BytesOut, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Network_PktsIn", component.Metrics.Network.PktsIn, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Network_PktsOut", component.Metrics.Network.PktsOut, labels);

                    // Process
                    _prometheusUtils.ReportGauge(Collectors, "Process_Run", component.Metrics.Process.Run, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Process_Total", component.Metrics.Process.Total, labels);

                    // Tracing
                    stopWatch.Stop();
                    _logger.LogInformation($"Runtime: {stopWatch.Elapsed}.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to export metrics.");
                throw;
            }
        }
    }
}
