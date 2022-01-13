using Kev.OpenWeather.Core.Interfaces;
using Kev.OpenWeather.Core.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Kev.OpenWeather.Core.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<WeatherService> _logger;

        public WeatherService(HttpClient httpClient, IConfiguration configuration, ILogger<WeatherService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<WeatherResponseDto> GetWetherResponseByCountryAndCity(string country, string city)
        {
            string appid = _configuration["ApiSettings:OpenWeatherKey"];

            var url = $"data/2.5/weather?q={city},{country}&appid={appid}";
            return await JsonSerializer.DeserializeAsync<WeatherResponseDto>
                (await _httpClient.GetStreamAsync(url), new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
        }
    }
}
