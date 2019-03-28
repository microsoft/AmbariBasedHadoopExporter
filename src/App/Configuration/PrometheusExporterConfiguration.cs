// <copyright file="PrometheusExporterConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Configuration
{
    using System.ComponentModel.DataAnnotations;
    using Core.Configurations;

    /// <summary>
    /// Placeholder for the Prometheus exporter settings.
    /// </summary>
    public class PrometheusExporterConfiguration : BaseValidatableConfiguration
    {
        /// <summary>
        /// Gets or sets the port number which will be exposed.
        /// </summary>
        [Required]
        [Range(1, 65535)]
        public int Port { get; set; }
    }
}
