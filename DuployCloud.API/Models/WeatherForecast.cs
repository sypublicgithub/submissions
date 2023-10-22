using System.Text.Json.Serialization;

namespace Duplocloud.API.Models
{
    /// <summary>
    /// Weather Forecast Summary
    /// </summary>
    public class WeatherForecast
    {
        /// <summary>
        /// Record ID
        /// </summary>
        public Guid? WeatherForecastId { get; set; }

        /// <summary>
        /// Latitude used for this forecast
        /// </summary>
        public float Latitude { get; set; }

        /// <summary>
        /// Longitude used for this forecast
        /// </summary>
        public float Longitude { get; set; }

        /// <summary>
        /// Forecast time
        /// </summary>
        public DateTime ForcastTime { get; set; }

        /// <summary>
        /// Temperature value and unit
        /// </summary>
        public string Temperature { get; set; }

        /// <summary>
        /// Relative humidity value and unit
        /// </summary>
        public string Humidity { get; set; }

        /// <summary>
        /// Windspeed value and unit
        /// </summary>
        public string Windspeed { get; set; }

        /// <summary>
        /// Raw JSON data captured from api call
        /// </summary>
        [JsonIgnore]
        public string? RawJsonData { get; internal set; }
    }
}
