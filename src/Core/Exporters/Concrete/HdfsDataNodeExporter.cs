// <copyright file="HdfsDataNodeExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Extensions;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Prometheus;

    /// <summary>
    /// Responsible for exporting HDFS DataNode metrics.
    /// </summary>
    internal class HdfsDataNodeExporter : BaseExporter
    {
        private readonly HdfsDataNodeExporterConfiguration _exporterConfiguration;

        public HdfsDataNodeExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<HdfsDataNodeExporterConfiguration> exporterConfiguration,
            ILogger<HdfsDataNodeExporter> logger)
            : base(contentProvider, prometheusUtils, exporterConfiguration.Value, typeof(HdfsDataNodeComponent), logger)
        {
            _exporterConfiguration = exporterConfiguration.Value;
        }

        /// <inheritdoc/>
        protected override async Task ReportMetrics(object component)
        {
            await Task.Factory.StartNew(() =>
            {
                var hdfsDataNodeComponent = component as HdfsDataNodeComponent;

                // Constructing labels
                var labels = new Dictionary<string, string>()
                {
                    { "ClusterName", hdfsDataNodeComponent.Info.ClusterName },
                    { "Component", hdfsDataNodeComponent.Info.ComponentName },
                };
                labels.TryAdd(_exporterConfiguration.DefaultLabels);

                // General info
                PrometheusUtils.ReportGauge(Collectors, "Info_StartedCount", hdfsDataNodeComponent.Info.StartedCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_TotalCount", hdfsDataNodeComponent.Info.TotalCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", hdfsDataNodeComponent.Info.UnknownCount, labels);

                // HdfsDataNode
                PrometheusUtils.ReportGauge(Collectors, "Capacity", hdfsDataNodeComponent.Metrics.HdfsDataNodeDfs.HdfsDataNode.Capacity, labels);
                PrometheusUtils.ReportGauge(Collectors, "DfsUsed", hdfsDataNodeComponent.Metrics.HdfsDataNodeDfs.HdfsDataNode.DfsUsed, labels);
                PrometheusUtils.ReportGauge(Collectors, "NumFailedVolumes", hdfsDataNodeComponent.Metrics.HdfsDataNodeDfs.HdfsDataNode.NumFailedVolumes, labels);

                // Cpu
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Idle", hdfsDataNodeComponent.Metrics.Cpu.Idle, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Nice", hdfsDataNodeComponent.Metrics.Cpu.Nice, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_System", hdfsDataNodeComponent.Metrics.Cpu.System, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_User", hdfsDataNodeComponent.Metrics.Cpu.User, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Wio", hdfsDataNodeComponent.Metrics.Cpu.Wio, labels);

                // HdfsDataNodeJvm
                PrometheusUtils.ReportGauge(Collectors, "Jvm_GcCount", hdfsDataNodeComponent.Metrics.DataNodeJvm.GcCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapCommittedM", hdfsDataNodeComponent.Metrics.DataNodeJvm.MemHeapCommittedM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapUsedM", hdfsDataNodeComponent.Metrics.DataNodeJvm.MemHeapUsedM, labels);

                // YarnResourceManagerRpc
                PrometheusUtils.ReportGauge(Collectors, "Rpc_NumOpenConnections", hdfsDataNodeComponent.Metrics.DataNodeRpc.NumOpenConnections, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_ProcessingTimeAvgTime", hdfsDataNodeComponent.Metrics.DataNodeRpc.ProcessingTimeAvgTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_QueueTimeAvgTime", hdfsDataNodeComponent.Metrics.DataNodeRpc.QueueTimeAvgTime, labels);

                // Disk
                PrometheusUtils.ReportGauge(Collectors, "Disk_Free", hdfsDataNodeComponent.Metrics.Disk.Free, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", hdfsDataNodeComponent.Metrics.Disk.ReadBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", hdfsDataNodeComponent.Metrics.Disk.ReadCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", hdfsDataNodeComponent.Metrics.Disk.ReadTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_Total", hdfsDataNodeComponent.Metrics.Disk.Total, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", hdfsDataNodeComponent.Metrics.Disk.WriteBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", hdfsDataNodeComponent.Metrics.Disk.WriteCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", hdfsDataNodeComponent.Metrics.Disk.WriteTime, labels);

                // Memory
                PrometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", hdfsDataNodeComponent.Metrics.Memory.CachedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", hdfsDataNodeComponent.Metrics.Memory.FreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SharedKb", hdfsDataNodeComponent.Metrics.Memory.SharedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", hdfsDataNodeComponent.Metrics.Memory.SwapFreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", hdfsDataNodeComponent.Metrics.Memory.TotalKb, labels);

                // Network
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesIn", hdfsDataNodeComponent.Metrics.Network.BytesIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesOut", hdfsDataNodeComponent.Metrics.Network.BytesOut, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsIn", hdfsDataNodeComponent.Metrics.Network.PktsIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsOut", hdfsDataNodeComponent.Metrics.Network.PktsOut, labels);

                // Process
                PrometheusUtils.ReportGauge(Collectors, "Process_Run", hdfsDataNodeComponent.Metrics.Process.Run, labels);
                PrometheusUtils.ReportGauge(Collectors, "Process_Total", hdfsDataNodeComponent.Metrics.Process.Total, labels);
            });
        }
    }
}
