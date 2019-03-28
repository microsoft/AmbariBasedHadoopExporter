// <copyright file="SecretsConfigurationSection.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Configuration
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Configurations;

    /// <summary>
    /// Defining the a configuration used to load the avaiable secrets to the service.
    /// </summary>
    public class SecretsConfigurationSection : BaseValidatableConfiguration
    {
        /// <summary>
        /// Gets or sets the absolute path of the secret folder, cannot be empty.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the secrets map that will be converted to configuration. The structure is as follows:
        ///     "Existing file name in the current directory": "Configuration_Section_Name--Configuration_Entry"
        /// </summary>
        public Dictionary<string, string> Mapping { get; set; }
    }
}
