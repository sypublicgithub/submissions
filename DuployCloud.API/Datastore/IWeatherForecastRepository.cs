using Duplocloud.API.Models;

namespace Duplocloud.API.Datastore
{
    /// <summary>
    /// Interface for WeatherForecast repository
    /// </summary>
    public interface IWeatherForecastRepository
    {
        /// <summary>
        /// Asynchronous get LongitudeLatitude history
        /// </summary>
        /// <returns></returns>
        Task<WeatherForecastRequest[]> GetAllLongitudeLatitudeHistory();

        /// <summary>
        /// Asynchronous get WeatherForecast history
        /// </summary>
        /// <returns></returns>
        Task<WeatherForecast[]> GetWeatherForecastHistory();

        /// <summary>
        /// Saving WeatherForecast into history table
        /// </summary>
        /// <param name="weatherForecast"></param>
        /// <returns></returns>
        Task<WeatherForecast> SaveWeatherForecast(WeatherForecast weatherForecast);

        /// <summary>
        /// Deleting WeatherForecast from history table
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteWeatherForecast(Guid id);        
    }
}
