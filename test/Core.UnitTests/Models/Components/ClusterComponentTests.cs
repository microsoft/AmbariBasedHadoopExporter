// <copyright file="ClusterComponentTests.cs" company="Microsoft">
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

    public class ClusterComponentTests
    {
        [Fact]
        public void Should_Load_Class_From_Json_Successfully()
        {
            var content = File.ReadAllText("Jsons/ClusterResponse.json");
            var component = JsonConvert.DeserializeObject<ClusterComponent>(content);

            component.Should().NotBeNull();
            component.ClusterReport.ClusterName.Should().Be("sparkpoc");
            component.ClusterReport.HealthReport.Should().NotBeNull();
            component.ClusterReport.HealthReport.HostsStatusHealthy.Should().Be(7);
            component.ClusterReport.HealthReport.HostsWithMaintenanceFlag.Should().Be(6);

            component.HostEntryList.Count.Should().Be(7);
            component.HostEntryList.First().Host.HostName.Should()
                .Be("hn0-sparkp.pk3tke12z5uejpo3re1mqa2wyf.fx.internal.cloudapp.net");

            component.AlertsSummary.Maintenance.Should().Be(55);
            component.AlertsSummaryHosts.Ok.Should().Be(6);
        }
    }
}
