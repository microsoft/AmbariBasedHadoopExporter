// <copyright file="DictionaryExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Extensions
{
    using System.Collections.Generic;

    internal static class DictionaryExtensions
    {
        /// <summary>
        /// Creating a dictionary from the default labels and an array of labels.
        /// </summary>
        /// <typeparam name="TKey">Key Type.</typeparam>
        /// <typeparam name="TValue">Value Type.</typeparam>
        /// <param name="dictionary">Dictionary of labels. Assertion - Cannot be null or empty.</param>
        /// <param name="defaults">Default labels.</param>
        internal static void TryAdd<TKey, TValue>(
            this Dictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> defaults)
        {
            if (defaults != null)
            {
                foreach (var(key, value) in defaults)
                {
                    dictionary.Add(key, value);
                }
            }
        }
    }
}
