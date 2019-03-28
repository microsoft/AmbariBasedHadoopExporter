// <copyright file="PrometheusExporterConfigurationTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.UnitTests.Configurations
{
    using System;
    using System.Collections.Generic;
    using App.Configuration;
    using FluentAssertions;
    using Xunit;

    public class PrometheusExporterConfigurationTests
    {
        public static readonly IEnumerable<object[]> TestCases = new List<object[]>
        {
            new object[]
            {
                new PrometheusExporterConfiguration
                {
                    Port = -1,
                },
                false,
            },
            new object[]
            {
                new PrometheusExporterConfiguration
                {
                    Port = 0,
                },
                false,
            },
            new object[]
            {
                new PrometheusExporterConfiguration
                {
                    Port = 80,
                },
                true,
            },
            new object[]
            {
                new PrometheusExporterConfiguration
                {
                    Port = 65535,
                },
                true,
            },
            new object[]
            {
                new PrometheusExporterConfiguration
                {
                    Port = 65536,
                },
                false,
            },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Validate_Attributes_Should_Pass(PrometheusExporterConfiguration conf, bool expectedResult)
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
