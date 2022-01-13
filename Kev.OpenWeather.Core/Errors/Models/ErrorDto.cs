using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Errors.Models
{
    /// <summary>
    /// Error
    /// </summary>
    public class ErrorDto
    {
        /// <summary>
        /// Error description for an user of application
        /// </summary>
        [JsonProperty(PropertyName = "userMessage")]
        public string UserMessage { get; set; }

        /// <summary>
        /// Error description for an engineer
        /// </summary>
        [JsonProperty(PropertyName = "internalMessage")]
        public string InternalMessage { get; set; }

        /// <summary>
        /// Unique code
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public int Code { get; set; }
    }
}
