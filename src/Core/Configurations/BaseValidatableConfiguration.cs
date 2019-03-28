// <copyright file="BaseValidatableConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    /// <summary>
    /// Base class for validating the configurations object.
    ///
    /// Example:
    /// Inherit from this class
    /// Add properties with  DataAnnotationsAttributes.
    /// Register the class as singleton which implements IValidatableConfiguration
    ///
    /// The framework will validate all configurations are valid on launch time
    /// </summary>
    public abstract class BaseValidatableConfiguration : IValidatableConfiguration
    {
        /// <inheritdoc/>
        public void Validate()
        {
            var errors = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(this, new ValidationContext(this), errors, true);

            if (!isValid)
            {
                throw new AggregateException(errors.Select(e => new ValidationException(e.ErrorMessage)));
            }
        }
    }
}
