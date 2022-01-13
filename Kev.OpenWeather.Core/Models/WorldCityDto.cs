using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Models
{


    public class WorldCityDto
    {
        [JsonProperty("id")]
        public string id { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("country")]
        public string country { get; set; }

    }

}
