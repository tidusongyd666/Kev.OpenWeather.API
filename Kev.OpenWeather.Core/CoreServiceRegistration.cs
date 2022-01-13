using Kev.OpenWeather.Core.Interfaces;
using Kev.OpenWeather.Core.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Reflection;

namespace Kev.OpenWeather.Core
{
    public static class CoreServiceRegistration
    {
        public static IServiceCollection AddCoreServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<HttpClient>(s =>
            {
                string baseUrl = configuration["ApiSettings:OpenWeatherBaseUrl"];
                var client = new HttpClient { BaseAddress = new System.Uri(baseUrl) };
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            });
            //DI
            services.AddScoped<IErrorsService, ErrorsService>();
            services.AddScoped<IWeatherService, WeatherService>();
            services.AddMediatR(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
