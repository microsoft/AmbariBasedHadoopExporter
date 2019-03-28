// <copyright file="YarnNodeManagerComponentTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Models
{
    using System.IO;
    using Core.Models.Components;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;

    public class YarnNodeManagerComponentTests
    {
        [Fact]
        public void Should_Load_Class_From_Json_Successfully()
        {
            var content = File.ReadAllText("Jsons/YarnNodeManagerResponse.json");
            var component = JsonConvert.DeserializeObject<NodeManagerComponent>(content);

            component.Should().NotBeNull();
            component.Info.ClusterName.Should().Be("sparkpoc");
            component.Info.ComponentName.Should().Be("NODEMANAGER");
            component.Info.StartTime.Should().Be(0);
            component.Info.StartedCount.Should().Be(2);
            component.Info.TotalCount.Should().Be(2);
            component.Info.UnknownCount.Should().Be(0);

            component.Metrics.YarnBase.AllocatedContainers.Should().Be(1.0);
            component.Metrics.YarnBase.AllocatedGB.Should().Be(1.5);
            component.Metrics.YarnBase.AllocatedVCores.Should().Be(1.0);
            component.Metrics.YarnBase.ContainersCompleted.Should().Be(261.0);
            component.Metrics.YarnBase.ContainersFailed.Should().Be(188.0);
            component.Metrics.YarnBase.ContainersIniting.Should().Be(0.0);
            component.Metrics.YarnBase.ContainersKilled.Should().Be(42.5);
            component.Metrics.YarnBase.ContainersLaunched.Should().Be(492.5);
            component.Metrics.YarnBase.ContainersRunning.Should().Be(1.0);

            component.Metrics.Cpu.Idle.Should().Be(67.1);
            component.Metrics.Cpu.Nice.Should().Be(25.03333333333334);
            component.Metrics.Cpu.System.Should().Be(3.0166666666666666);
            component.Metrics.Cpu.User.Should().Be(2.6500000000000004);
            component.Metrics.Cpu.Wio.Should().Be(2.133333333333333);

            component.Metrics.Disk.Free.Should().Be(1116.5133333333333);
            component.Metrics.Disk.ReadBytes.Should().Be(1010818531157.3333);
            component.Metrics.Disk.ReadCount.Should().Be(36989319.166666664);
            component.Metrics.Disk.ReadTime.Should().Be(11379552);
            component.Metrics.Disk.Total.Should().Be(1166.09);
            component.Metrics.Disk.WriteBytes.Should().Be(2137495063893.3335);
            component.Metrics.Disk.WriteCount.Should().Be(153274067.66666666);
            component.Metrics.Disk.WriteTime.Should().Be(390232664.6666666);

            component.Metrics.Memory.CachedKb.Should().Be(10199195.333333332);
            component.Metrics.Memory.FreeKb.Should().Be(10832333.333333332);
            component.Metrics.Memory.SharedKb.Should().Be(0.0);
            component.Metrics.Memory.SwapFreeKb.Should().Be(6881148.0);
            component.Metrics.Memory.TotalKb.Should().Be(14362160.0);

            component.Metrics.Network.BytesIn.Should().Be(35262.061601662645);
            component.Metrics.Network.BytesOut.Should().Be(22768.010268507038);
            component.Metrics.Network.PktsIn.Should().Be(40.06216396742174);
            component.Metrics.Network.PktsOut.Should().Be(29.67923292219633);

            component.Metrics.Process.Run.Should().Be(1.0);
            component.Metrics.Process.Total.Should().Be(179.0);
        }
    }
}
