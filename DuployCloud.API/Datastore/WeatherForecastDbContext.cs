using Duplocloud.API.Datastore.DBModels;
using Microsoft.EntityFrameworkCore;

namespace Duplocloud.API.Datastore
{
    /// <summary>
    /// DbContext for WeatherForecast EF database
    /// </summary>
    public class WeatherForecastDbContext : DbContext
    {
        private readonly IConfiguration Configuration;

        /// <summary>
        /// Entities for storing historic weather forecast data
        /// </summary>
        public virtual DbSet<WeatherForecastHistory> WeatherForecasts { get; set; }

        /// <summary>
        /// DbContext for WeatherForecast EF database
        /// </summary>
        /// <param name="configuration">injected configuration</param>
        /// <exception cref="ArgumentNullException"></exception>
        public WeatherForecastDbContext(IConfiguration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException("IConfiguration");
        }

        /// <summary>
        /// Override configuration
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(this.Configuration.GetConnectionString("SqliteDbConnectionString"));
        }
    }
}
