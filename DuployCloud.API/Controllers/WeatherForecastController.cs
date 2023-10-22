using Duplocloud.API.Datastore;
using Duplocloud.API.Models;
using Duplocloud.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace duplocloud.Controllers;

/// <summary>
/// API Controller for getting weather forecasts
/// </summary>
[ApiController]
[Route("weatherforecasts/")]
public class WeatherForecastController : ControllerBase
{
    private readonly IWeatherForecastResolver WeatherForecastResolver;
    private readonly IWeatherForecastRepository Repository;

    /// <summary>
    /// API Controller for getting weather forecasts
    /// </summary>
    /// <param name="weatherForecastResolver">Injected forecast resolver</param>
    /// <param name="repository">Injected repository</param>
    /// <exception cref="ArgumentNullException">throws exception if missing injected instances</exception>
    public WeatherForecastController(
        IWeatherForecastResolver weatherForecastResolver,
        IWeatherForecastRepository repository)
    {
        this.WeatherForecastResolver = weatherForecastResolver ?? throw new ArgumentNullException("IWeatherForecastResolver");
        this.Repository = repository ?? throw new ArgumentNullException("IWeatherForecastRepository");
    }

    /// <summary>
    /// Get a weather forecast from logitude and latitude
    /// </summary>
    /// <returns>Returns a weather forecast</returns>
    [HttpGet]
    [Route("logitude/{logitude}/latitude/{latitude}", Name = "GetWeatherForecast")]
    public async Task<WeatherForecast> Get(float logitude = 13.41f, float latitude = 52.52f)
    {
        var forecast = await this.WeatherForecastResolver.GetWeatherForecast(
            new WeatherForecastRequest
            {
                longitude = logitude,
                latitude = latitude
            });

        await this.Repository.SaveWeatherForecast(forecast);
        return forecast;
    }

    /// <summary>
    ///  Get a weather forecast from logitude and latitude by POST request
    /// </summary>
    /// <param name="request">WeatherForecastRequest</param>
    /// <returns>Returns a weather forecast</returns>
    [HttpPost(Name = "GetWeatherForecastByPost")]
    public Task<WeatherForecast> Post(WeatherForecastRequest request)
    {
        return this.Get(request.longitude, request.latitude);
    }

    /// <summary>
    /// Get a list of longitude and latitude request history
    /// </summary>
    /// <returns></returns>
    [HttpGet("requests/history", Name = "GetLongitudeLatitudeHistory")]
    public async Task<WeatherForecastRequest[]> GetLongitudeLatitudeHistory()
    {
        var weatherForecasts = await this.Repository.GetAllLongitudeLatitudeHistory();
        return weatherForecasts;
    }

    /// <summary>
    /// Get a list of historic weather forecasts
    /// </summary>
    /// <returns></returns>
    [HttpGet("forecasts/history", Name = "GetWeatherForecastHistory")]
    public async Task<WeatherForecast[]> GetWeatherForecastHistory()
    {
        var weatherForecasts = await this.Repository.GetWeatherForecastHistory();
        return weatherForecasts;
    }

    /// <summary>
    /// Delete a stored weather forcast by its Id
    /// </summary>
    /// <param name="WeatherForecastId">Weather Forecast ID</param>
    /// <returns>Http status code</returns>
    [HttpDelete("{WeatherForecastId}", Name = "DeleteWeatherForecast")]
    public async Task<StatusCodeResult> Delete(Guid WeatherForecastId)
    {
        var isSuccess = await this.Repository.DeleteWeatherForecast(WeatherForecastId);
        return StatusCode(isSuccess ? (int)HttpStatusCode.OK : (int)HttpStatusCode.NotModified);
    }
}
