using System.ComponentModel.DataAnnotations;

namespace Duplocloud.API.Datastore.DBModels
{
    /// <summary>
    /// Matabase model, used to DbContext
    /// </summary>
    public class WeatherForecastHistory
    {
        public WeatherForecastHistory()
        {
            this.Created = DateTime.UtcNow;
        }

        [Key]
        public Guid Id { get; set; }
        public float Longitude { get; set; }
        public float Latitude { get; set; }
        public DateTime ForcastTime { get; set; }
        public string Temperature { get; set; }
        public string Humidity { get; set; }
        public string Windspeed { get; set; }
        public string RawJsonData { get; set; }
        public DateTime Created { get; set; }
    }
}
