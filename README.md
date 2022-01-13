# Open Weather API in ASP.NET Core

## [Live Demo - API Swagger UI](https://api-kev-weather-ae66.azurewebsites.net/swagger/index.html)

You can play with the API using Swagger UI

## Solution Structure

This solution contains 3 projects.

### 1 `Kev.OpenWeather.API - An ASP.NET Core Web API project`
- Get city weather API endpoint: `/api/v1/getweather/country/{country}/city/{city}` 
  - This one enabled rate limit
- Search countries API endpoint: `/api/v1/getweather/countries?search={search}&page={page}&size={size}`  
  - No rate limit
- Search cities API endpoint: `/api/v1/getweather/cities?search={search}&page={page}&size={size}`  
  - No rate limit
- **Important!**: Please add custom header for API authrization **e.g. X_ClientId:cl-key-1**
- API limited Keys 
  - ["cl-key-1", "cl-key-2", "cl-key-3", "cl-key-4", "cl-key-5"]  
  - Each one is limited to 5 calls per hour 
- API unlimited Key ["cl-key-no-limit"] 
  - Allow unlimited calls for demo purpose only 


### 2 `Kev.OpenWeather.Core - A .NET Core Class Project`
- Contains models, interfaces, services, features and others classes and resources to encapulate application logical using by API project

### 3 `Kev.OpenWeather.Test - A .NET Core Unit Test Project `
- A .Net Core test project including below automatic tests
- TestCountrySearch
- TestCitySearch
- TestClientRateLimitOptions
- TestGetCityWeather
- TestGetWeatherWithUnlimitedKey
- ExpectExceptionWhenExceedRateLimitWithLimitedKey
- ExpectUnauthorizedException
- ExpectBadRequestException


## Tech stack
- .NET Core 3.1
- ASP.NET Core 3.1
- Azure Web App (Live Demo Host)

## Run


## Testing
### Manual: 
 - [API Swagger UI](https://api-kev-weather-ae66.azurewebsites.net/swagger/index.html)
OR
### Automatic: 
 - Run Unit Test Project

## Place Holder 1


## Place Holder 2


## Place Holder 3



