using Kev.OpenWeather.Core.Errors;
using Kev.OpenWeather.Core.Features.GetWeather;
using Kev.OpenWeather.Core.Features.Lookups.GetCities;
using Kev.OpenWeather.Core.Features.Lookups.GetCountires;
using Kev.OpenWeather.Core.Interfaces;
using Kev.OpenWeather.Core.Models;
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
using System.IO;
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

        [HttpGet("getweather/country/{country}/city/{city}", Name = "getweatherbycountrycity")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<WeatherDescriptionVm>> GetWeatherbyCountryCity(string country, string city, [FromHeader] string X_ClientId = "cl-key-1")
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


        [HttpGet("getweather/cities", Name = "getcities")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagedCityiesVm>> GetCities(int page = 1, int size = 10, string search = "", [FromHeader] string X_ClientId = "cl-key-no-limit")
        {
            if (!ConfigManager.configRoot.GetSection("ClientIds").Get<string[]>().Contains(X_ClientId))
                return Unauthorized(this.errorService.CreateError(ErrorCode.AuthenticationFailed));

            var getCitiesForSearch = new GetCitiesForSearchQuery() { Search = search, Page = page, Size = size };
            var vms = await mediator.Send(getCitiesForSearch);

            return Ok(vms);
        }


        [HttpGet("getweather/countries", Name = "getcountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<PagedCountriesVm>> GetCountries(int page = 1, int size = 10, string search = "", [FromHeader] string X_ClientId = "cl-key-no-limit")
        {
            if (!ConfigManager.configRoot.GetSection("ClientIds").Get<string[]>().Contains(X_ClientId))
                return Unauthorized(this.errorService.CreateError(ErrorCode.AuthenticationFailed));

            var getCountriesForSearch = new GetCountriesForSearchQuery() { Search = search, Page = page, Size = size };
            var vms = await mediator.Send(getCountriesForSearch);

            return Ok(vms);
        }


    }
}
