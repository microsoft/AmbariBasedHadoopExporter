// <copyright file="IPrometheusUtils.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Utils
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using Prometheus;

    /// <summary>
    /// Utility functions used to report metrics to Prometheus.
    /// </summary>
    public interface IPrometheusUtils
    {
        /// <summary>
        /// Reporting a Gauge to prometheus and persisting it to the input dictionary.
        /// </summary>
        /// <param name="cache">Collectiong storing all reported Gauges.</param>
        /// <param name="metricName">Metric name.</param>
        /// <param name="metricValue">Metric value.</param>
        /// <param name="labels">DefaultLabels of the metric.</param>
        /// <param name="helpMessage">[Optional] Help message for the metric.</param>
        void ReportGauge(
            ConcurrentDictionary<string, Collector> cache,
            string metricName,
            double metricValue,
            Dictionary<string, string> labels,
            string helpMessage = "");
    }
}
