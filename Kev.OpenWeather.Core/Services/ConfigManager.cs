using Kev.OpenWeather.Core.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kev.OpenWeather.Core.Services
{
    public class ConfigManager
    {
        public static IConfiguration configRoot { set; get; }
        public static IList<WorldCityDto> cityLookups { get; set; }
    }
}
