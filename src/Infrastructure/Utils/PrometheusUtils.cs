// <copyright file="PrometheusUtils.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using Core.Utils;
    using Prometheus;

    /// <inheritdoc />
    internal class PrometheusUtils : IPrometheusUtils
    {
        /// <inheritdoc />
        public void ReportGauge(
            ConcurrentDictionary<string, Collector> cache,
            string metricName,
            double metricValue,
            Dictionary<string, string> labels,
            string helpMessage = "")
        {
            if (cache == null ||
                metricName == null || metricName.Equals(string.Empty) ||
                labels == null)
            {
                throw new ArgumentException($"Invalid arguments were passed." +
                                            $"Dictionary - cannot be null: {cache}," +
                                            $"MetricName - cannot be null or empty: {metricName}," +
                                            $"MetricValue - cannot be lower than 0: {metricValue}," +
                                            $"Labels - cannot be null: {labels}," +
                                            $"HelpMessage - cannot be null: {helpMessage}.");
            }

            helpMessage = helpMessage ?? string.Empty;

            // Adding or getting the gauge and reporting it.
            ((Gauge)cache.GetOrAdd(
                    metricName,
                    f => Metrics.CreateGauge(metricName, helpMessage, new GaugeConfiguration().LabelNames = labels.Keys.ToArray())))
                .WithLabels(labels.Values.ToArray())
                .Set(metricValue);
        }
    }
}
