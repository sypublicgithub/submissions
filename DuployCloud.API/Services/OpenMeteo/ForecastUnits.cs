using System.Text.Json.Serialization;

namespace Duplocloud.API.Services.OpenMeteo
{
    /// <summary>
    /// OpenMeteo forecast units JSON model
    /// </summary>
    public class ForecastUnits
    {
        [JsonPropertyName("temperature_2m")]
        public string Temperature { get; set; }

        [JsonPropertyName("relativehumidity_2m")]
        public string RelativeHumidity { get; set; }

        [JsonPropertyName("windspeed_10m")]
        public string WindSpeed { get; set; }
    }
}
