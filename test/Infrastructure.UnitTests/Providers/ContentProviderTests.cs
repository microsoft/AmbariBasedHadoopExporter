// <copyright file="ContentProviderTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.UnitTests.Providers
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Infrastructure.Configuration;
    using Infrastructure.Providers.Concrete;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class ContentProviderTests
    {
        private readonly Mock<IOptions<AmbariClientConfiguration>> _configuration;
        private readonly Mock<ILogger<AmbariApiContentProvider>> _logger;
        private readonly AmbariApiContentProvider _ambariApiContentProvider;

        public ContentProviderTests()
        {
            var ambariConf = new AmbariClientConfiguration
            {
                Username = "Test",
                Password = "Test",
            };

            _configuration = new Mock<IOptions<AmbariClientConfiguration>>();
            _configuration.Setup(f => f.Value).Returns(ambariConf);
            _logger = new Mock<ILogger<AmbariApiContentProvider>>();

            _ambariApiContentProvider = new AmbariApiContentProvider(_configuration.Object, _logger.Object);
        }

        [Fact]
        public void Should_Raise_Exception()
        {
            Func<Task> func = async () => { await _ambariApiContentProvider.GetResponseContentAsync("InvalidUri"); };
            func.Should().Throw<Exception>();
        }

        [Fact]
        public void Should_Raise_Forbidden_Exception()
        {
            Func<Task> func = async () => { await _ambariApiContentProvider.GetResponseContentAsync("https://sparkpoc.azurehdinsight.net/api/v1/clusters/sparkpoc"); };

            func.Should().Throw<HttpRequestException>();
            func.Should().Throw<HttpRequestException>().WithMessage("Status code: Forbidden isnt Ok.");
        }

        [Fact]
        public void Should_Raise_NoHost_Exception()
        {
            Func<Task> func = async () => { await _ambariApiContentProvider.GetResponseContentAsync("https://dontexist123123.azurehdinsight.net/api/v1/clusters/dontexist123123"); };

            func.Should().Throw<HttpRequestException>();
        }
    }
}
