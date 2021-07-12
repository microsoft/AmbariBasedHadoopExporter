// <copyright file="DictionaryExtensionsTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using FluentAssertions;
    using Xunit;

    public class DictionaryExtensionsTests
    {
        public static IEnumerable<object[]> Data => new List<object[]>
        {
            new object[]
            {
                null,
                null,
                false,
                -1,
            },
            new object[]
            {
                null,
                new Dictionary<string, string>(),
                false,
                -1,
            },
            new object[]
            {
                new Dictionary<string, string>(),
                new Dictionary<string, string>(),
                true,
                0,
            },
            new object[]
            {
                new Dictionary<string, string>(),
                new Dictionary<string, string>() { { "Key", "Value" } },
                true,
                1,
            },
            new object[]
            {
                new Dictionary<string, string>(),
                new Dictionary<string, string>() { { "Key", "Value" }, { "Key2", "Value2" } },
                true,
                2,
            },
            new object[]
            {
                new Dictionary<string, string>() { { "Key", "Value" } },
                new Dictionary<string, string>(),
                true,
                1,
            },
            new object[]
            {
                new Dictionary<string, string>() { { "Key", "Value" } },
                new Dictionary<string, string>() { { "Key2", "Value2" } },
                true,
                2,
            },
            new object[]
            {
                new Dictionary<string, string>() { { "Key", "Value" } },
                new Dictionary<string, string>() { { "Key", "Value" } },
                false,
                -1,
            },
        };

        [Theory]
        [MemberData(nameof(Data))]
        public void Adding_Valid_Data_Should_Run_Successfully(
            Dictionary<string, string> baseDictionary,
            Dictionary<string, string> addedDictionary,
            bool shouldRunSuccessfully,
            int countAssertion)
        {
            try
            {
                baseDictionary.TryAdd(addedDictionary);
                baseDictionary.Count().Should().Be(countAssertion);
            }
            catch (Exception e)
            {
                if (shouldRunSuccessfully)
                {
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                    throw;
                }
            }
        }
    }
}
