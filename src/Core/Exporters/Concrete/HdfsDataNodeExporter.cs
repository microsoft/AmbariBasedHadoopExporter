// <copyright file="HdfsDataNodeExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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
    /// Responsible for exporting HDFS DataNode metrics.
    /// </summary>
    internal class HdfsDataNodeExporter : IExporter
    {
        private readonly IContentProvider _contentProvider;
        private readonly IPrometheusUtils _prometheusUtils;
        private readonly HdfsDataNodeExporterConfiguration _exporterConfiguration;
        private readonly ILogger<HdfsDataNodeExporter> _logger;

        public HdfsDataNodeExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<HdfsDataNodeExporterConfiguration> exporterConfiguration,
            ILogger<HdfsDataNodeExporter> logger)
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
                var content = await _contentProvider.GetResponseContentAsync(_exporterConfiguration.UriEndpoint);
                var component = JsonConvert.DeserializeObject<HdfsDataNodeComponent>(content);

                // Constructing labels
                var labels = new Dictionary<string, string>()
                    {
                        { "ClusterName", component.Info.ClusterName },
                        { "Component", component.Info.ComponentName },
                    };
                labels.TryAdd(_exporterConfiguration.DefaultLabels);

                // General info
                _prometheusUtils.ReportGauge(Collectors, "Info_StartedCount", component.Info.StartedCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Info_TotalCount", component.Info.TotalCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", component.Info.UnknownCount, labels);

                // HdfsDataNode
                _prometheusUtils.ReportGauge(Collectors, "Capacity", component.Metrics.HdfsDataNodeDfs.HdfsDataNode.Capacity, labels);
                _prometheusUtils.ReportGauge(Collectors, "DfsUsed", component.Metrics.HdfsDataNodeDfs.HdfsDataNode.DfsUsed, labels);
                _prometheusUtils.ReportGauge(Collectors, "NumFailedVolumes", component.Metrics.HdfsDataNodeDfs.HdfsDataNode.NumFailedVolumes, labels);

                // Cpu
                _prometheusUtils.ReportGauge(Collectors, "Cpu_Idle", component.Metrics.Cpu.Idle, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_Nice", component.Metrics.Cpu.Nice, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_System", component.Metrics.Cpu.System, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_User", component.Metrics.Cpu.User, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_Wio", component.Metrics.Cpu.Wio, labels);

                // HdfsDataNodeJvm
                _prometheusUtils.ReportGauge(Collectors, "Jvm_GcCount", component.Metrics.DataNodeJvm.GcCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapCommittedM", component.Metrics.DataNodeJvm.MemHeapCommittedM, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapUsedM", component.Metrics.DataNodeJvm.MemHeapUsedM, labels);

                // YarnResourceManagerRpc
                _prometheusUtils.ReportGauge(Collectors, "Rpc_NumOpenConnections", component.Metrics.DataNodeRpc.NumOpenConnections, labels);
                _prometheusUtils.ReportGauge(Collectors, "Rpc_ProcessingTimeAvgTime", component.Metrics.DataNodeRpc.ProcessingTimeAvgTime, labels);
                _prometheusUtils.ReportGauge(Collectors, "Rpc_QueueTimeAvgTime", component.Metrics.DataNodeRpc.QueueTimeAvgTime, labels);

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
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to export metrics.");
                throw;
            }
        }
    }
}
