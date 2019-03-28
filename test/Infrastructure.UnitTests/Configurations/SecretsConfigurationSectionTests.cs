// <copyright file="SecretsConfigurationSectionTests.cs" company="Microsoft">
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

    public class SecretsConfigurationSectionTests
    {
        public static readonly IEnumerable<object[]> TestCases = new List<object[]>
        {
            new object[]
            {
                new SecretsConfigurationSection
                {
                    Path = string.Empty,
                },
                false,
            },
            new object[]
            {
                new SecretsConfigurationSection
                {
                    Path = "some/text",
                },
                true,
            },
            new object[]
            {
                new SecretsConfigurationSection
                {
                    Path = "some/text",
                    Mapping = null,
                },
                true,
            },
            new object[]
            {
                new SecretsConfigurationSection
                {
                    Path = "some/text",
                    Mapping = new Dictionary<string, string>(),
                },
                true,
            },
            new object[]
            {
                new SecretsConfigurationSection
                {
                    Path = "some/text",
                    Mapping = new Dictionary<string, string>()
                    {
                        { "Key", "Value" },
                    },
                },
                true,
            },
        };

        [Theory]
        [MemberData(nameof(TestCases))]
        public void Validate_Attributes_Should_Pass(SecretsConfigurationSection conf, bool expectedResult)
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
