// <copyright file="Ugi.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class Ugi
    {
        [JsonProperty(PropertyName = "loginFailure_avg_time")]
        public double LoginFailure_avg_time { get; set; }

        [JsonProperty(PropertyName = "loginFailure_num_ops")]
        public long LoginFailure_num_ops { get; set; }

        [JsonProperty(PropertyName = "loginSuccess_avg_time")]
        public double LoginSuccess_avg_time { get; set; }

        [JsonProperty(PropertyName = "loginSuccess_num_ops")]
        public long LoginSuccess_num_ops { get; set; }
    }
}
