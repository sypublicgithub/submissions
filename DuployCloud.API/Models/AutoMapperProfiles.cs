using AutoMapper;
using Duplocloud.API.Datastore.DBModels;
using Duplocloud.API.Services.OpenMeteo;

namespace Duplocloud.API.Models
{
    /// <summary>
    /// Configure AutoMapper
    /// </summary>
    public class AutoMapperProfiles : Profile
    {
        /// <summary>
        /// Initialize Maps for automapper, see builder.Services.AddAutoMapper at program start 
        /// </summary>
        public AutoMapperProfiles()
        {
            // Mappins for OpenMeteo to dto
            CreateMap<ForecastSummary, WeatherForecast>()
                .ForMember(f => f.Humidity, s => s.MapFrom(u => string.Format($"{u.Forecast.Humidity} {u.CurrentUnits.RelativeHumidity}")))
                .ForMember(f => f.Temperature, s => s.MapFrom(u => string.Format($"{u.Forecast.Temperature} {u.CurrentUnits.Temperature}")))
                .ForMember(f => f.Windspeed, s => s.MapFrom(u => string.Format($"{u.Forecast.Windspeed} {u.CurrentUnits.WindSpeed}")))
                .ForMember(f => f.ForcastTime, s => s.MapFrom(u => u.Forecast.Time));
            
            // Mappings for db model mapping to dto
            CreateMap<WeatherForecastHistory, WeatherForecastRequest>();
            CreateMap<WeatherForecastHistory, WeatherForecast>()
                .ForMember(f => f.WeatherForecastId, s => s.MapFrom(u => u.Id));

            // Mappings for dto to database model
            CreateMap<WeatherForecast, WeatherForecastHistory>();
        }
    }
}
