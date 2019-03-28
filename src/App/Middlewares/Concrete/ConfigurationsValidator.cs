// <copyright file="ConfigurationsValidator.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Middlewares.Concrete
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using App.Middlewares.Abstract;
    using Core.Configurations;

    /// <inheritdoc/>
    public class ConfigurationsValidator : IValidator
    {
        private readonly IEnumerable<IValidatableConfiguration> _validatableObjects;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationsValidator"/> class.
        /// </summary>
        /// <param name="validatableObjects">Enumerable of all configurations that implement IValidatableConfiguration.</param>
        public ConfigurationsValidator(IEnumerable<IValidatableConfiguration> validatableObjects)
        {
            _validatableObjects = validatableObjects;
        }

        /// <inheritdoc/>
        public async Task ValidateAsync()
        {
            if (_validatableObjects == null)
            {
                return;
            }

            foreach (var validatableObject in _validatableObjects)
            {
                await Task.Run(() => validatableObject.Validate());
            }
        }
    }
}
