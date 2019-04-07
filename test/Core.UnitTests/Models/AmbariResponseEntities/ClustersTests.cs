// <copyright file="ClustersTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Models.AmbariResponseEntities
{
    using System.IO;
    using Core.Models.AmbariResponseEntities.Cluster;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;

    public class ClustersTests
    {
        [Fact]
        public void Should_Load_Class_From_Json_Successfully()
        {
            var content = File.ReadAllText("Jsons/ClustersResponse.json");
            var component = JsonConvert.DeserializeObject<Clusters>(content);

            component.Should().NotBeNull();
            component.ClusterList.Count.Should().Be(2);
            component.ClusterList[0].Info.Name = "testCluster";
            component.ClusterList[1].Info.Name = "testClusterTwo";
        }
    }
}
