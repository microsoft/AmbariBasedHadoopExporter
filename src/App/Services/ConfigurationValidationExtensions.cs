// <copyright file="ConfigurationValidationExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using App.Middlewares.Abstract;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Extension methods for <see cref="IHost"/>, running configuration validation.
    /// </summary>
    public static class ConfigurationValidationExtensions
    {
        /// <summary>
        /// Getting all classes that implement the IValidationStartupFilter and invokes their validation.
        /// </summary>
        /// <param name="host">IHost object.</param>
        /// <returns>Task performing the validation.</returns>
        public static async Task ValidateConfigurationAsync(this IHost host)
        {
            var startupConfigurations = host.Services.GetService<IEnumerable<IValidator>>();
            foreach (var startupConfiguration in startupConfigurations)
            {
                await startupConfiguration.ValidateAsync();
            }
        }
    }
}
