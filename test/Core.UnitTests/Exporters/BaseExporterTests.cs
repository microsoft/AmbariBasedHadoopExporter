// <copyright file="BaseExporterTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Exporters
{
    using System;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Exporters.Concrete;
    using Core.Providers;
    using Core.Utils;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class BaseExporterTests
    {
        [Fact]
        public void IllegalAruments_Should_Throw()
        {
            // Using YarnResourceManagerExporter as a placeholder for one of the implementing classes
            var contentProvider = new Mock<IContentProvider>();
            var prometheusUtils = new Mock<IPrometheusUtils>();

            var configurationOptions = new Mock<IOptions<YarnResourceManagerExporterConfiguration>>();
            var configuration = new Mock<YarnResourceManagerExporterConfiguration>();
            configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            configurationOptions.Setup(f => f.Value).Returns(configuration.Object);

            var logger = new Mock<ILogger<YarnResourceManagerExporter>>();

            var exporter = new YarnResourceManagerExporter(
                contentProvider.Object,
                prometheusUtils.Object,
                configurationOptions.Object,
                logger.Object);
            contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));

            // Empty string
            Func<Task> func = async () => { await exporter.ExportMetricsAsync(string.Empty); };
            func.Should().Throw<ArgumentException>();

            // Invalid string format
            func = async () => { await exporter.ExportMetricsAsync("/someString"); };
            func.Should().Throw<ArgumentException>();
        }
    }
}
