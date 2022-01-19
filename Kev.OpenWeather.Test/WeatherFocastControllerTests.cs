using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using Kev.OpenWeather.API;
using Kev.OpenWeather.Core.Features.GetWeather;
using Kev.OpenWeather.Core.Features.Lookups.GetCities;
using Kev.OpenWeather.Core.Features.Lookups.GetCountires;
using Kev.OpenWeather.Core.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace Kev.OpenWeather.Test
{
    [TestClass]
    public class WeatherFocastControllerTests
    {
        #region Lookups Apis
        [DataTestMethod]
        [DataRow("au", 1, 1, 1)]
        [DataRow("", 1, 1000, 247)]
        [DataRow("wrong", 1, 10, 0)]
        public async Task TestCountrySearch(string search, int page, int size, int expectedCount)
        {
            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            string unlimitedClientId = options.Value.ClientWhitelist.FirstOrDefault();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", unlimitedClientId);
            var response = await httpClient.GetAsync($"api/v1/getweather/countries?search={search}&page={page}&size={size}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<PagedCountriesVm>(result);
            Assert.AreEqual(expectedCount, vm.Count);
        }

        [DataTestMethod]
        [DataRow("Melbourne", 1, 1000, 13)]
        [DataRow("", 1, 999999, 209579)]
        [DataRow("wrong", 1, 10, 0)]
        public async Task TestCitySearch(string search, int page, int size, int expectedCount)
        {
            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            string unlimitedClientId = options.Value.ClientWhitelist.FirstOrDefault();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", unlimitedClientId);
            var response = await httpClient.GetAsync($"api/v1/getweather/cities?search={search}&page={page}&size={size}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<PagedCityiesVm>(result);
            Assert.AreEqual(expectedCount, vm.Count);
        }

        [DataTestMethod]
        [DataRow("au", -1, 10, 1)]
        [DataRow("au", 1, -10, 1)]
        [DataRow("au", -1, -10, 2)]
        [DataRow("more than ten characters", 1, 10, 1)]
        public async Task TestSearchCountryiesWithInvalidParams(string search, int page, int size, int expectedCount)
        {

            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            string unlimitedClientId = options.Value.ClientWhitelist.FirstOrDefault();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", unlimitedClientId);
            var response = await httpClient.GetAsync($"api/v1/getweather/countries?search={search}&page={page}&size={size}");
            var result = await response.Content.ReadAsStringAsync();
            var validationError = JsonConvert.DeserializeObject<List<string>>(result);
            Assert.AreEqual(expectedCount, validationError.Count);
  
        }

        [DataTestMethod]
        [DataRow("brisbane", -1, 10, 1)]
        [DataRow("brisbane", 1, -10, 1)]
        [DataRow("brisbane", -1, -10, 2)]
        public async Task TestSearchCitiesWithInvalidParams(string search, int page, int size, int expectedCount)
        {

            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            string unlimitedClientId = options.Value.ClientWhitelist.FirstOrDefault();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", unlimitedClientId);
            var response = await httpClient.GetAsync($"api/v1/getweather/cities?search={search}&page={page}&size={size}");
            var result = await response.Content.ReadAsStringAsync();
            var validationError = JsonConvert.DeserializeObject<List<string>>(result);
            Assert.AreEqual(expectedCount, validationError.Count);

        }


        #endregion

        #region getWeather Api and Client RateLimit

        [TestMethod]
        public void TestClientRateLimitOptions()
        {
            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            Assert.AreEqual(1, options.Value.GeneralRules.Count);
            var generalRule = options.Value.GeneralRules[0];
            Assert.AreEqual("*:/api/*", generalRule.Endpoint);
            Assert.AreEqual("1h", generalRule.Period);
            Assert.AreEqual(5, generalRule.Limit);
        }

        [TestMethod]
        public async Task TestGetCityWeather()
        {
            string city = "Brisbane";
            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            string unlimitedClientId = options.Value.ClientWhitelist.FirstOrDefault();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", unlimitedClientId);
            var response = await httpClient.GetAsync($"api/v1/getweather/country/au/city/{city}");
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            var vm = JsonConvert.DeserializeObject<WeatherDescriptionVm>(result);
            
            Assert.IsTrue(!string.IsNullOrEmpty(vm.Description));

        }

        [TestMethod]
        public async Task TestGetWeatherWithUnlimitedKey()
        {
            try
            {
                using var factory = new WebApplicationFactory<Startup>();
                var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
                string unlimitedClientId = options.Value.ClientWhitelist.FirstOrDefault();
                var httpClient = factory.CreateClient();
                httpClient.DefaultRequestHeaders.Add("X_ClientId", unlimitedClientId);
                //Test 6 calls
                var cities = new List<string> { "Brisbane", "Brisbane", "Brisbane", "Brisbane", "Brisbane", "Brisbane" };
                var allTasks = cities.Select(city => Task.Run(async () =>
                {
                    var result = await httpClient.GetStringAsync($"api/v1/getweather/country/au/city/{city}");
                    Console.WriteLine($"{city} is ? {result}");
                })).ToList();
                await Task.WhenAll(allTasks);
            }
            catch (Exception ex)
            {
                Assert.Fail("Expected no exception, but got: " + ex.Message);
            }
            Assert.IsTrue(true);

        }


        [TestMethod]
        public async Task ExpectExceptionWhenExceedRateLimitWithLimitedKey()
        {
           
            using var factory = new WebApplicationFactory<Startup>();
            var options = factory.Services.GetRequiredService<IOptions<ClientRateLimitOptions>>();
            string limitedClientId = ConfigManager.configRoot.GetSection("ClientIds").Get<string[]>().FirstOrDefault();

            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", limitedClientId);
            //Test 6 calls
            var cities = new List<string> { "Brisbane", "Brisbane", "Brisbane", "Brisbane", "Brisbane", "Brisbane" };
            var allTasks = cities.Select(city => Task.Run(async () =>
            {
                var result = await httpClient.GetStringAsync($"api/v1/getweather/country/au/city/{city}");
                Console.WriteLine($"{city} is ? {result}");
            })).ToList();
            async Task ConcurrentApiRequests() => await Task.WhenAll(allTasks);
            var e = await Assert.ThrowsExceptionAsync<HttpRequestException>(ConcurrentApiRequests);
            Assert.AreEqual("Response status code does not indicate success: 429 (Too Many Requests).", e.Message);
        }

        

        [TestMethod]
        public async Task ExpectUnauthorizedException()
        {
            string city = "Brisbane";
            string wrongClientId = "wrong-key";
            using var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", wrongClientId);

            var e = await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await httpClient.GetStringAsync($"api/v1/getweather/country/au/city/{city}"));
            Assert.AreEqual("Response status code does not indicate success: 401 (Unauthorized).", e.Message);

        }

        [TestMethod]
        public async Task ExpectBadRequestException()
        {
            string country = "Wrong County";
            string city = "Wrong City";
            using var factory = new WebApplicationFactory<Startup>();
            var httpClient = factory.CreateClient();
            httpClient.DefaultRequestHeaders.Add("X_ClientId", "cl-key-no-limit");

            var e = await Assert.ThrowsExceptionAsync<HttpRequestException>(async () => await httpClient.GetStringAsync($"api/v1/getweather/country/{country}/city/{city}"));
            Assert.AreEqual("Response status code does not indicate success: 400 (Bad Request).", e.Message);
           
        }

        #endregion

    }
}
