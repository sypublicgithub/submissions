namespace Duplocloud.API.Configurations
{
    /// <summary>
    /// POCO for binding configuration in appsettings.json
    /// </summary>
    public class OpenMeteoConfig
    {
        /// <summary>
        /// Url for the OpenMeteo forecast api
        /// </summary>
        public virtual string WeatherForecastUrl { get; set; }

        /// <summary>
        /// Default configuration for OpenMeteo, specifying fields returned from query
        /// </summary>
        public virtual string CurrentForecastQueryParams { get; set; }
    }
}
