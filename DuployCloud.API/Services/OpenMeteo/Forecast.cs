using System.Text.Json.Serialization;

namespace Duplocloud.API.Services.OpenMeteo
{
    /// <summary>
    /// OpenMeteo forecast metrics JSON model
    /// </summary>
    public class Forecast
    {
        public DateTime Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public float Temperature { get; set; }

        [JsonPropertyName("relativehumidity_2m")]
        public float Humidity { get; set; }

        [JsonPropertyName("windspeed_10m")]
        public float Windspeed { get; set; }
    }
}
