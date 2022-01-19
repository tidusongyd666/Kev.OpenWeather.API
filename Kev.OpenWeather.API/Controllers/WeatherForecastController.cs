using Kev.OpenWeather.Core.Errors;
using Kev.OpenWeather.Core.Errors.Models;
using Kev.OpenWeather.Core.Features.GetWeather;
using Kev.OpenWeather.Core.Features.Lookups.GetCities;
using Kev.OpenWeather.Core.Features.Lookups.GetCountires;
using Kev.OpenWeather.Core.Interfaces;
using Kev.OpenWeather.Core.Services;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kev.OpenWeather.API.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class WeatherForecastController : ControllerBase
    {
        private IErrorsService errorService;
        private IWeatherService weatherService;
        private IWebHostEnvironment env;
        private ILogger<WeatherForecastController> logger;
        private readonly IMediator mediator;

        public WeatherForecastController(IErrorsService _errorService,
            IWeatherService _weatherService,
            IWebHostEnvironment _env,
            ILogger<WeatherForecastController> _logger,
            IMediator _mediator)
        {
            this.errorService = _errorService;
            this.weatherService = _weatherService;
            this.env = _env;
            this.logger = _logger;
            this.mediator = _mediator;
        }

        /// <summary>
        /// Get Weather Report (Note: this endpoint has enabled client rate limit 5 calls per hour for each X_ClientId)
        /// </summary>
        /// <remarks> Get weather report by country and city</remarks>
        /// <param name="country">e.g. au</param>
        /// <param name="city">e.g. brisbane</param>
        /// <param name="X_ClientId">cl-key-1, cl-key-2, cl-key-3, cl-key-4, cl-key-5, cl-key-no-limit</param> 
        [HttpGet("getweather/country/{country}/city/{city}", Name = "getweatherbycountrycity")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(WeatherDescriptionVm))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDto))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<WeatherDescriptionVm>> GetWeatherbyCountryCity([FromHeader] string X_ClientId, string country, string city)
        {
            if (!ConfigManager.configRoot.GetSection("ClientIds").Get<string[]>().Contains(X_ClientId))
                return Unauthorized(this.errorService.CreateError(ErrorCode.AuthenticationFailed));

            var validationResult = ConfigManager.cityLookups.Where(v => v.country.Equals(country, StringComparison.OrdinalIgnoreCase) && v.name.Equals(city, StringComparison.OrdinalIgnoreCase)).Any();

            if(!validationResult) 
                return BadRequest(this.errorService.CreateError(ErrorCode.CountryOrCityIsNotValid));

            var resp = await weatherService.GetWetherResponseByCountryAndCity(country, city);

            var desc = new WeatherDescriptionVm() { Description = resp.weather?.Count() > 0 ? resp.weather.FirstOrDefault().description ?? "" : "" };

            return Ok(desc);
        }

        /// <summary>
        /// Execute Cities Search (Note: this endpoint has no client rate limit)
        /// </summary>
        /// <remarks>Execute cities search with pagenations</remarks>
        /// <param name="search">e.g. brisbane</param>
        /// <param name="page">1</param>
        /// <param name="size">10</param>
        /// <param name="X_ClientId">cl-key-1, cl-key-2, cl-key-3, cl-key-4, cl-key-5, cl-key-no-limit</param>   
        [HttpGet("getweather/cities", Name = "getcities")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedCityiesVm))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagedCityiesVm>> GetCities([FromHeader] string X_ClientId, int page = 1, int size = 10, string search = "")
        {
            if (!ConfigManager.configRoot.GetSection("ClientIds").Get<string[]>().Contains(X_ClientId))
                return Unauthorized(this.errorService.CreateError(ErrorCode.AuthenticationFailed));

            var getCitiesForSearch = new GetCitiesForSearchQuery() { Search = search, Page = page, Size = size };
            var vms = await mediator.Send(getCitiesForSearch);

            return Ok(vms);
        }

        /// <summary>
        /// Execute Countries Search (Note: this endpoint has no client rate limit)
        /// </summary>
        /// <remarks>Execute countries search with pagenations</remarks>
        /// <param name="search">e.g. au</param>
        /// <param name="page">1</param>
        /// <param name="size">10</param>
        /// <param name="X_ClientId">cl-key-1, cl-key-2, cl-key-3, cl-key-4, cl-key-5, cl-key-no-limit</param>   
        [HttpGet("getweather/countries", Name = "getcountries")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedCountriesVm))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorDto))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<string>))]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagedCountriesVm>> GetCountries([FromHeader] string X_ClientId, int page = 1, int size = 10, string search = "")
        {
            if (!ConfigManager.configRoot.GetSection("ClientIds").Get<string[]>().Contains(X_ClientId))
                return Unauthorized(this.errorService.CreateError(ErrorCode.AuthenticationFailed));

            var getCountriesForSearch = new GetCountriesForSearchQuery() { Search = search, Page = page, Size = size };
            var vms = await mediator.Send(getCountriesForSearch);

            return Ok(vms);
        }


    }
}
