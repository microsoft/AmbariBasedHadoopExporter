// <copyright file="LivenessConfigurationTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.UnitTests.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using App.Configuration;
    using FluentAssertions;
    using Xunit;

    public class LivenessConfigurationTests
    {
        public static readonly IEnumerable<object[]> TestCases = new List<object[]>
        {
            new object[]
            {
                new LivenessConfiguration
                {
                    SamplingPeriodInSeconds = -1,
                },
                false,
            },
            new object[]
            {
                new LivenessConfiguration
                {
                    SamplingPeriodInSeconds = 0,
                },
                false,
            },
            new object[]
            {
                new LivenessConfiguration
                {
                    SamplingPeriodInSeconds = 10,
                },
                true,
            },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Validate_Attributes_Should_Pass(LivenessConfiguration conf, bool expectedResult)
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
