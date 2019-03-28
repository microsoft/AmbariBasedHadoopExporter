// <copyright file="YarnResourceManagerComponentTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Models
{
    using System.IO;
    using System.Linq;
    using Core.Models.Components;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;

    public class YarnResourceManagerComponentTests
    {
        [Fact]
        public void Should_Load_Class_From_Json_Successfully()
        {
            var content = File.ReadAllText("Jsons/YarnResourceManagerResponse.json");
            var component = JsonConvert.DeserializeObject<ResourceManagerComponent>(content);

            component.Should().NotBeNull();
            component.Info.ClusterName.Should().Be("sparkpoc");
            component.Info.ComponentName.Should().Be("RESOURCEMANAGER");
            component.Info.StartTime.Should().Be(1542881623405);
            component.Info.StartedCount.Should().Be(2);
            component.Info.TotalCount.Should().Be(2);
            component.Info.UnknownCount.Should().Be(0);

            component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumActiveNMs.Should().Be(2);
            component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumDecommissionedNMs.Should().Be(0);
            component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumLostNMs.Should().Be(0);
            component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumRebootedNMs.Should().Be(0);
            component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumUnhealthyNMs.Should().Be(0);

            component.Metrics.YarnMetrics.Queues.Root.AMResourceLimitMB.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.AMResourceLimitVCores.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.ActiveApplications.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.ActiveUsers.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.AggregateContainersAllocated.Should().Be(1371);
            component.Metrics.YarnMetrics.Queues.Root.AggregateContainersReleased.Should().Be(1369);
            component.Metrics.YarnMetrics.Queues.Root.AllocatedContainers.Should().Be(2);
            component.Metrics.YarnMetrics.Queues.Root.AllocatedMB.Should().Be(3072);
            component.Metrics.YarnMetrics.Queues.Root.AllocatedVCores.Should().Be(2);
            component.Metrics.YarnMetrics.Queues.Root.AppAttemptFirstContainerAllocationDelayAvgTime.Should().Be(0.0);
            component.Metrics.YarnMetrics.Queues.Root.AppAttemptFirstContainerAllocationDelayNumOps.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.AppsCompleted.Should().Be(114);
            component.Metrics.YarnMetrics.Queues.Root.AppsFailed.Should().Be(10);
            component.Metrics.YarnMetrics.Queues.Root.AppsKilled.Should().Be(21);
            component.Metrics.YarnMetrics.Queues.Root.AppsPending.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.AppsRunning.Should().Be(2);
            component.Metrics.YarnMetrics.Queues.Root.AppsSubmitted.Should().Be(147);
            component.Metrics.YarnMetrics.Queues.Root.AvailableMB.Should().Be(21504);
            component.Metrics.YarnMetrics.Queues.Root.AvailableVCores.Should().Be(12);
            component.Metrics.YarnMetrics.Queues.Root.PendingContainers.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.PendingMB.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.PendingVCores.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.ReservedContainers.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.ReservedMB.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.ReservedVCores.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.UsedAMResourceMB.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.UsedAMResourceVCores.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.Running0.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.Running1440.Should().Be(2);
            component.Metrics.YarnMetrics.Queues.Root.Running300.Should().Be(0);
            component.Metrics.YarnMetrics.Queues.Root.Running60.Should().Be(0);

            var queues = component.Metrics.YarnMetrics.Queues.Root.GetChildrenQueuesIEnumerable();
            queues.Count().Should().Be(2);
            var defaultQueue = queues.First();
            defaultQueue.AMResourceLimitMB.Should().Be(7168);
            defaultQueue.AMResourceLimitVCores.Should().Be(1);
            defaultQueue.ActiveApplications.Should().Be(0);
            defaultQueue.ActiveUsers.Should().Be(0);
            defaultQueue.AggregateContainersAllocated.Should().Be(1355);
            defaultQueue.AggregateContainersReleased.Should().Be(1355);
            defaultQueue.AllocatedContainers.Should().Be(0);
            defaultQueue.AllocatedMB.Should().Be(0);
            defaultQueue.AllocatedVCores.Should().Be(0);
            defaultQueue.AppAttemptFirstContainerAllocationDelayAvgTime.Should().Be(0.0);
            defaultQueue.AppAttemptFirstContainerAllocationDelayNumOps.Should().Be(0);
            defaultQueue.AppsCompleted.Should().Be(100);
            defaultQueue.AppsFailed.Should().Be(9);
            defaultQueue.AppsKilled.Should().Be(21);
            defaultQueue.AppsPending.Should().Be(0);
            defaultQueue.AppsRunning.Should().Be(0);
            defaultQueue.AppsSubmitted.Should().Be(130);
            defaultQueue.AvailableMB.Should().Be(12288);
            defaultQueue.AvailableVCores.Should().Be(7);
            defaultQueue.PendingContainers.Should().Be(0);
            defaultQueue.PendingMB.Should().Be(0);
            defaultQueue.PendingVCores.Should().Be(0);
            defaultQueue.ReservedContainers.Should().Be(0);
            defaultQueue.ReservedMB.Should().Be(0);
            defaultQueue.ReservedVCores.Should().Be(0);
            defaultQueue.UsedAMResourceMB.Should().Be(0);
            defaultQueue.UsedAMResourceVCores.Should().Be(0);
            defaultQueue.Running0.Should().Be(0);
            defaultQueue.Running1440.Should().Be(0);
            defaultQueue.Running300.Should().Be(0);
            defaultQueue.Running60.Should().Be(0);

            component.Metrics.ResourceManagerJvm.HeapMemoryMax.Should().Be(954728448);
            component.Metrics.ResourceManagerJvm.HeapMemoryUsed.Should().Be(137754320);
            component.Metrics.ResourceManagerJvm.NonHeapMemoryMax.Should().Be(-1);
            component.Metrics.ResourceManagerJvm.NonHeapMemoryUsed.Should().Be(118702384);
            component.Metrics.ResourceManagerJvm.GcCount.Should().Be(151301);
            component.Metrics.ResourceManagerJvm.GcTimeMillis.Should().Be(859527);
            component.Metrics.ResourceManagerJvm.LogError.Should().Be(7);
            component.Metrics.ResourceManagerJvm.LogFatal.Should().Be(0);
            component.Metrics.ResourceManagerJvm.LogInfo.Should().Be(44826);
            component.Metrics.ResourceManagerJvm.LogWarn.Should().Be(2187);
            component.Metrics.ResourceManagerJvm.MemHeapCommittedM.Should().Be(155.5);
            component.Metrics.ResourceManagerJvm.MemHeapUsedMB.Should().Be(147.84479);
            component.Metrics.ResourceManagerJvm.MemMaxMB.Should().Be(910.5);
            component.Metrics.ResourceManagerJvm.MemNonHeapCommittedMB.Should().Be(116.0);
            component.Metrics.ResourceManagerJvm.MemNonHeapUsedMB.Should().Be(113.203415);
            component.Metrics.ResourceManagerJvm.ThreadsBlocked.Should().Be(0);
            component.Metrics.ResourceManagerJvm.ThreadsNew.Should().Be(0);
            component.Metrics.ResourceManagerJvm.ThreadsRunnable.Should().Be(19);
            component.Metrics.ResourceManagerJvm.ThreadsTerminated.Should().Be(0);
            component.Metrics.ResourceManagerJvm.ThreadsTimedWaiting.Should().Be(176);
            component.Metrics.ResourceManagerJvm.ThreadsWaiting.Should().Be(73);

            // YarnResourceManagerRpc
            component.Metrics.ResourceManagerYarnResourceManagerRpc.NumOpenConnections.Should().Be(2);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.ReceivedBytes.Should().Be(353595791);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcProcessingTime_avg_time.Should().Be(0.0);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcProcessingTime_num_ops.Should().Be(2155869);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcQueueTime_avg_time.Should().Be(0.0);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcQueueTime_num_ops.Should().Be(2155869);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.SentBytes.Should().Be(104076987);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.CallQueueLen.Should().Be(0);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthenticationFailures.Should().Be(0);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthenticationSuccesses.Should().Be(216);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthorizationFailures.Should().Be(0);
            component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthorizationSuccesses.Should().Be(216);

            // Runtime
            component.Metrics.Runtime.StartTime.Should().Be(1542881623405);

            // Ugi
            component.Metrics.Ugi.LoginFailure_avg_time.Should().Be(0.0);
            component.Metrics.Ugi.LoginFailure_num_ops.Should().Be(0);
            component.Metrics.Ugi.LoginSuccess_avg_time.Should().Be(0.0);
            component.Metrics.Ugi.LoginSuccess_num_ops.Should().Be(0);
        }
    }
}
