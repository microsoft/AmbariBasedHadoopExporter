// <copyright file="IValidator.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Middlewares.Abstract
{
    using System.Threading.Tasks;

    /// <summary>
    /// A validator, validating the state of other objects. Intended for configuration objects validation
    /// before consumption.
    /// </summary>
    public interface IValidator
    {
        /// <summary>
        /// Task that validates all registered classes.
        /// </summary>
        /// <returns>Task that performs the validation.</returns>
        Task ValidateAsync();
    }
}
