using System.Text.Json.Serialization;

namespace Duplocloud.API.Services.OpenMeteo
{
    /// <summary>
    /// OpenMeteo forecast error response JSON model
    /// </summary>
    public class ForecastErrorResponse
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }

        [JsonPropertyName("reason")]
        public string Reason { get; set; }
    }
}
