// <copyright file="AmbariClientConfigurationTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.UnitTests.Configurations
{
    using System;
    using System.Collections.Generic;
    using FluentAssertions;
    using Infrastructure.Configuration;
    using Xunit;

    public class AmbariClientConfigurationTests
    {
        public static readonly IEnumerable<object[]> TestCases = new List<object[]>
        {
            new object[]
            {
                new AmbariClientConfiguration
                {
                    Username = "user",
                    Password = string.Empty,
                },
                false,
            },
            new object[]
            {
                new AmbariClientConfiguration
                {
                    Username = string.Empty,
                    Password = "pass",
                },
                false,
            },
            new object[]
            {
                new AmbariClientConfiguration
                {
                    Username = "user",
                    Password = "pass",
                },
                true,
            },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Validate_Attributes_Should_Pass(AmbariClientConfiguration conf, bool expectedResult)
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
