namespace Duplocloud.API.Models
{
    /// <summary>
    /// Model for requesting weather forecast using longitude and latitude
    /// </summary>
    public class WeatherForecastRequest
    {
        /// <summary>
        /// longitude input
        /// </summary>
        public float longitude { get; set; }

        /// <summary>
        /// latitude input
        /// </summary>
        public float latitude { get; set; }
    }
}
