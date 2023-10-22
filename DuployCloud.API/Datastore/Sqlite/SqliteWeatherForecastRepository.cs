using AutoMapper;
using Duplocloud.API.Datastore.DBModels;
using Duplocloud.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Duplocloud.API.Datastore.Sqlite
{
    /// <summary>
    /// Sqlite repository
    /// </summary>
    public class SqliteWeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly IConfiguration Configuration;
        private readonly ILogger<SqliteWeatherForecastRepository> Logger;
        private readonly IMapper Mapper;

        /// <summary>
        /// Sqlite repository
        /// </summary>
        /// <param name="configuration">Injected configuration</param>
        /// <param name="logger">Injected logger</param>
        /// <param name="mapper">Injected mapper</param>
        /// <exception cref="ArgumentNullException">Exception thrown if injected instance is null</exception>
        public SqliteWeatherForecastRepository(
            IConfiguration configuration,
            ILogger<SqliteWeatherForecastRepository> logger,
            IMapper mapper)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException("IConfiguration");
            this.Logger = logger ?? throw new ArgumentNullException("ILogger");
            this.Mapper = mapper ?? throw new ArgumentNullException("IMapper");
        }

        /// <summary>
        /// Saving WeatherForecast into history table
        /// </summary>
        /// <param name="weatherForecast"></param>
        /// <returns></returns>
        public async Task<WeatherForecast> SaveWeatherForecast(WeatherForecast weatherForecast)
        {
            var dbModel = this.Mapper.Map<WeatherForecastHistory>(weatherForecast);
            using (var dbContext = new WeatherForecastDbContext(this.Configuration))
            {

                dbContext.WeatherForecasts.Add(dbModel);
                await dbContext.SaveChangesAsync();

                weatherForecast.WeatherForecastId = dbModel.Id;
            }

            return weatherForecast;
        }

        /// <summary>
        /// Asynchronous get LongitudeLatitude history
        /// </summary>
        /// <returns></returns>
        public async Task<WeatherForecastRequest[]> GetAllLongitudeLatitudeHistory()
        {
            using (var dbContext = new WeatherForecastDbContext(this.Configuration))
            {

                var weatherForecastHistories = await dbContext.WeatherForecasts
                        .GroupBy(d => new { d.Longitude, d.Latitude })
                        .Select(group => group.First())
                        .ToArrayAsync();

                return this.Mapper.Map<WeatherForecastRequest[]>(weatherForecastHistories);
            }
        }

        /// <summary>
        /// Asynchronous get WeatherForecast history
        /// </summary>
        /// <returns></returns>
        public async Task<WeatherForecast[]> GetWeatherForecastHistory()
        {
            using (var dbContext = new WeatherForecastDbContext(this.Configuration))
            {

                var weatherForecastHistories = await dbContext.WeatherForecasts
                    .OrderByDescending(x => x.Created)
                    .ToArrayAsync();

                return this.Mapper.Map<WeatherForecast[]>(weatherForecastHistories);
            }
        }

        /// <summary>
        /// Deleting WeatherForecast from history table
        /// </summary>
        /// <param name="id"></param>
        /// <returns>true is successful or false is error on saving</returns>
        public async Task<bool> DeleteWeatherForecast(Guid id)
        {
            try
            {
                using (var dbContext = new WeatherForecastDbContext(this.Configuration))
                {
                    var found = await dbContext.WeatherForecasts.FindAsync(id);
                    if (found != null)
                    {
                        dbContext.WeatherForecasts.Remove(found);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.ToString());
                return false;
            }

            return true;
        }

        //public async Task ResetDatabase()
        //{
        //    using (var dbContext = new WeatherForecastDbContext(this.Configuration))
        //    {
        //        dbContext.Database.EnsureDeleted();
        //        dbContext.Database.EnsureCreated();
        //    }
        //}
    }
}
