# Open Weather API in ASP.NET Core

## [Live Demo - API Swagger UI](https://api-kev-weather-ae66.azurewebsites.net/swagger/index.html)

Be free to play with the API using Swagger UI

## Solution Structure

This solution contains 3 projects.


### 1 `Kev.OpenWeather.API - An ASP.NET Core Web API project`
- Get weather API endpoint: `/api/v1/getweather/country/{country}/city/{city}` 
  - This one has enabled rate limit 5 times per hour by the X_ClientId in request header
- Search countries API endpoint: `/api/v1/getweather/countries?search={search}&page={page}&size={size}`  
  - No rate limit
  - Counties Search support pagination, wildcard search and case insensitive
- Search cities API endpoint: `/api/v1/getweather/cities?search={search}&page={page}&size={size}`  
  - No rate limit
  - Cities Search support pagination, wildcard search and case insensitive
  - Cities and Countires data downloaded form OpenWeather website, for the details please check the [Link](https://bulk.openweathermap.org/sample/)
- API limited Keys 
  - 5 limited Keys can be used, each key is limited to 5 calls per hour 
- API unlimited Key 
  - Allow unlimited calls to get weather API. It is for demo purpose only 

**Important!**: Please add custom header for API Key authrization **e.g. X_ClientId:cl-key-1** 
, you can use below keys as needed

| Key Name       | Key Value       | Type  |      
| -------------  |:---------------:|:-----------:|
| X_ClientId     | cl-key-1        |Limited Key  |
| X_ClientId     | cl-key-2        |Limited Key  |
| X_ClientId     | cl-key-3        |Limited Key  |
| X_ClientId     | cl-key-4        |Limited Key  |
| X_ClientId     | cl-key-5        |Limited Key  |
| X_ClientId     | cl-key-no-limit |Unlimited Key|



### 2 `Kev.OpenWeather.Core - A .NET Core Class Project`
- Contains models, interfaces, services, features and infrastructure code using by API project.

### 3 `Kev.OpenWeather.Test - A .NET Core Unit Test Project `
Including below automatic tests
- TestCountrySearch
- TestCitySearch
- TestSearchCountryiesWithInvalidParams
- TestSearchCitiesWithInvalidParams
- TestClientRateLimitOptions
- TestGetCityWeather
- TestGetWeatherWithUnlimitedKey
- ExpectExceptionWhenExceedRateLimitWithLimitedKey
- ExpectUnauthorizedException
- ExpectBadRequestException

## Tech Stack
- .NET Core 3.1
- ASP.NET Core 3.1
- Azure Web App (Live Demo Host)
- Third party Libraries  
  - AspNetCoreRateLimit
  - MediatR
  - Serilog
  - Swashbuckle.AspNetCore


## Build & Run 
 - Clone or donwload project
 - Open Solution by Visual Studio 2019 or Later
 - Waiting for all packages installed
 - Select solution then right click to rebuild
 - Press Ctrl+F5 to run without the debugger OR F5 with the debugger.
 - It will launch your default browser to Swagger UI http://localhost:5000/swagger/index.html

## Testing
### Manual: 
- Using [API Swagger UI](https://api-kev-weather-ae66.azurewebsites.net/swagger/index.html)
- Using Postman, you can download the collection file from the [Link](https://drive.google.com/file/d/1cXUC_uPFokl9jaEbbYN-bQO_D46X2WOL/view?usp=sharing)
### Automatic: 
 - Clone or donwload project
 - Open Solution by Visual Studio 2019 or Later
 - Open Test Explorer from view menu
 - Select Test Project
 - Right Click the test project to show context menu
 - Click run test menu item
 - Check the test result in Test Explorer

