using System.Text.Json.Serialization;

namespace Duplocloud.API.Services.OpenMeteo
{
    /// <summary>
    /// OpenMeteo forecast summary JSON model
    /// </summary>
    public class ForecastSummary
    {
        public float Latitude { get; set; }

        public float Longitude { get; set; }

        public string Timezone { get; set; }

        [JsonPropertyName("current")]
        public Forecast Forecast { get; set; }

        [JsonPropertyName("current_units")]
        public ForecastUnits CurrentUnits { get; set; }
    }
}
