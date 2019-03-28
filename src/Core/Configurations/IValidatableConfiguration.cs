// <copyright file="IValidatableConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Configurations
{
    /// <summary>
    /// A validatable configuration object, intended to be validated before consumption.
    /// </summary>
    public interface IValidatableConfiguration
    {
        /// <summary>
        /// Performing a validation operation.
        /// <exception cref="System.ComponentModel.DataAnnotations.ValidationException">Throws ValidationException.</exception>
        /// </summary>
        void Validate();
    }
}
