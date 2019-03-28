// <copyright file="PrometheusUtilsTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.UnitTests.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using FluentAssertions;
    using Infrastructure.Utils;
    using Prometheus;
    using Xunit;

    public class PrometheusUtilsTests
    {
        private PrometheusUtils _promtheusUtils;

        public PrometheusUtilsTests()
        {
            _promtheusUtils = new PrometheusUtils();
        }

        [Fact]
        public void Adding_New_Gague_Should_Run_Successfully()
        {
            var collectors = new ConcurrentDictionary<string, Collector>();
            var labels = new Dictionary<string, string>();
            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameNoLabelsOne",
                10,
                labels);
            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameNoLabelsTwo",
                10,
                labels);

            collectors.Count.Should().Be(2);
        }

        [Fact]
        public void Adding_New_Gague_With_Labels_Should_Run_Successfully()
        {
            var collectors = new ConcurrentDictionary<string, Collector>();
            var labels = new Dictionary<string, string>()
            {
                { "Name", "Value" },
            };

            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameWithLabelsOne",
                10,
                labels);
            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameWithLabelsTwo",
                10,
                labels);

            collectors.Count.Should().Be(2);
        }

        [Fact]
        public void Updating_Existing_Gaguge_Should_Run_Successfully()
        {
            var collectors = new ConcurrentDictionary<string, Collector>();
            var labels = new Dictionary<string, string>()
            {
                { "Name", "Value" },
            };

            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameUpdatedWithLabelsOne",
                10,
                labels);
            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameUpdatedWithLabelsTwo",
                10,
                labels);

            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameUpdatedWithLabelsOne",
                20,
                labels);
            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameUpdatedWithLabelsTwo",
                25,
                labels);

            collectors.Count.Should().Be(2);

            Collector collector;
            collectors.TryGetValue("metricNameUpdatedWithLabelsOne", out collector).Should().BeTrue();
            collector.As<Gauge>().WithLabels(labels.Values.ToArray()).Value.Should().Be(20);

            collectors.TryGetValue("metricNameUpdatedWithLabelsTwo", out collector).Should().BeTrue();
            collector.As<Gauge>().WithLabels(labels.Values.ToArray()).Value.Should().Be(25);
        }

        [Fact]
        public void Updating_Existing_Gaguge_With_Different_Labels_Should_Fail()
        {
            var collectors = new ConcurrentDictionary<string, Collector>();
            var labels = new Dictionary<string, string>()
            {
                { "Name", "Value" },
            };

            _promtheusUtils.ReportGauge(
                collectors,
                "metricNameUpdatedError",
                10,
                new Dictionary<string, string>());

            Action action = () =>
                _promtheusUtils.ReportGauge(
                    collectors,
                    "metricNameUpdatedError",
                    20,
                    labels);

            action.Should().Throw<Exception>();
        }
    }
}
