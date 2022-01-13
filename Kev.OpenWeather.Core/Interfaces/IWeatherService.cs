using Kev.OpenWeather.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kev.OpenWeather.Core.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWetherResponseByCountryAndCity(string country, string city);
    }
}
