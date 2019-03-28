// <copyright file="ISecretProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Providers.Abstract
{
    using System.Collections.Generic;

    /// <summary>
    /// Exposing set of function used to access secrets.
    /// </summary>
    internal interface ISecretProvider
    {
        /// <summary>
        /// Generates a dictionary holding {SecretName: SecretValue}.
        /// </summary>
        /// <returns>Read only dictionary.</returns>
        IReadOnlyDictionary<string, string> GetSecretNameToValueMap();
    }
}
