// <copyright file="AmbariClientConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Configuration
{
    using System.ComponentModel.DataAnnotations;
    using Core.Configurations;

    /// <summary>
    /// Ambari HTTP client configuration.
    /// </summary>
    public class AmbariClientConfiguration : BaseValidatableConfiguration
    {
        /// <summary>
        /// Gets or sets the username that will be used to authenticate to Ambari.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password that will be used to authenticate to Ambari.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Password { get; set; }
    }
}
