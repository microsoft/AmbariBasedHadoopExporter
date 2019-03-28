// <copyright file="BaseExporterConfigurationTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Configurations.Exporters
{
    using System;
    using System.Collections.Generic;
    using Core.Configurations.Exporters;
    using FluentAssertions;
    using Xunit;

    public class BaseExporterConfigurationTests
    {
        public static readonly IEnumerable<object[]> TestCases = new List<object[]>
        {
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "testCluster",
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                },
                true,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "Hello!",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "Hello!",
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "Hello",
                    AmbariServerUri = "hhttp://someuri",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "Hello.com",
                    AmbariServerUri = "http://someuri",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "Hello",
                    AmbariServerUri = "http://someuri/api/v1/clusterss",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "Th1sIsB4d",
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                },
                false,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "testCluster",
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                    DefaultLabels = null,
                },
                true,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "testCluster",
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                    DefaultLabels = new Dictionary<string, string>(),
                },
                true,
            },
            new object[]
            {
                new ClusterExporterConfiguration
                {
                    ClusterName = "testCluster",
                    AmbariServerUri = "http://someuri/api/v1/clusters",
                    DefaultLabels = new Dictionary<string, string>()
                    {
                        { "LabelName", "Value" },
                    },
                },
                true,
            },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Validate_Attributes_Should_Pass(ClusterExporterConfiguration conf, bool expectedResult)
        {
            try
            {
                conf.Validate();
            }
            catch (Exception e)
            {
                if (expectedResult)
                {
                    throw;
                }

                e.Should().BeOfType<AggregateException>();
            }
        }
    }
}
