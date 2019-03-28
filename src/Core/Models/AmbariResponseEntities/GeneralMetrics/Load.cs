// <copyright file="Load.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.GeneralMetrics
{
    using Newtonsoft.Json;

    public class Load
    {
        [JsonProperty(PropertyName = "load_fifteen")]
        public double Fifteen { get; set; }

        [JsonProperty(PropertyName = "load_five")]
        public double Five { get; set; }

        [JsonProperty(PropertyName = "load_one")]
        public double One { get; set; }
    }
}
