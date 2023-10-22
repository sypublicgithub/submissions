using Duplocloud.API.Models;

namespace Duplocloud.API.Services
{
    /// <summary>
    /// Interface for Weather Forecast Resolvers
    /// </summary>
    public interface IWeatherForecastResolver
    {
        /// <summary>
        /// Returns current weather forecast
        /// </summary>
        /// <param name="request">WeatherForecastRequest</param>
        /// <returns>WeatherForecastSummary</returns>
        Task<WeatherForecast> GetWeatherForecast(WeatherForecastRequest request);
    }
}
