using AutoMapper;
using Duplocloud.API.Configurations;
using Duplocloud.API.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using RestSharp;
using System.Text.Json;

namespace Duplocloud.API.Services.OpenMeteo
{
    /// <summary>
    /// Makes API call to OpenMeteo for current forecast
    /// </summary>
    public class OpenMeteoForecastResolver : IWeatherForecastResolver
    {
        private readonly OpenMeteoConfig OpenMeteoConfig;
        private readonly ILogger<OpenMeteoForecastResolver> Logger;
        private readonly IMapper Mapper;

        /// <summary>
        /// Gets the base url for OpenMeteo Forecast API
        /// </summary>
        public string BaseUrl => OpenMeteoConfig.WeatherForecastUrl;

        /// <summary>
        /// Makes API call to OpenMeteo for current forecast
        /// </summary>
        /// <param name="OpenMeteoConfig">Injected configuration</param>
        /// <param name="logger">Inject logger</param>
        /// <param name="mapper">Inject mapper</param>
        /// <exception cref="ArgumentNullException"></exception>
        public OpenMeteoForecastResolver(
            IOptions<OpenMeteoConfig> OpenMeteoConfig,
            ILogger<OpenMeteoForecastResolver> logger,
            IMapper mapper)
        {
            this.OpenMeteoConfig = OpenMeteoConfig?.Value ?? throw new ArgumentNullException("IOptions");
            this.Logger = logger ?? throw new ArgumentNullException("ILogger");
            this.Mapper = mapper ?? throw new ArgumentNullException("IMapper");
        }

        /// <summary>
        /// Returns current weather forecast
        /// </summary>
        /// <param name="request">WeatherForecastRequest</param>
        /// <returns>WeatherForecast</returns>
        public async Task<WeatherForecast> GetWeatherForecast(WeatherForecastRequest request)
        {
            try
            {
                var resourceQuery = GetCurrentForecastQuery(request.longitude, request.latitude);
                var response = await CallRestClient(resourceQuery);
                if (!response.IsSuccessful)
                {
                    var error = JsonSerializer.Deserialize<ForecastErrorResponse>(response.Content!);
                    throw new InvalidRequestException(error?.Reason!);
                }

                var openMeteoForecastSummary = JsonSerializer.Deserialize<ForecastSummary>(
                      response.Content!,
                      new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                  )!;

                var mapped = this.Mapper.Map<WeatherForecast>(openMeteoForecastSummary);
                mapped.RawJsonData = response.Content;
                return mapped;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Unexpected Error");
                throw;
            }
        }

        /// <summary>
        /// Get the current query parameters for current forecast query
        /// </summary>
        /// <param name="longitude">longitude query value</param>
        /// <param name="latitude">latitude query value</param>
        /// <returns></returns>
        protected virtual string GetCurrentForecastQuery(float longitude, float latitude)
        {
            // ?latitude=52.52&longitude=13.41&current=temperature_2m,relativehumidity_2m,windspeed_10m
            var uri = new Uri(QueryHelpers.AddQueryString(
                BaseUrl,
                new Dictionary<string, string?>() {
                { "latitude", latitude.ToString() },
                { "longitude", longitude.ToString() },
                { "current",  OpenMeteoConfig.CurrentForecastQueryParams },
            }));

            return uri.Query;
        }

        /// <summary>
        /// Test stub 
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<RestResponse> CallRestClient(string resourceQuery)
        {
            var client = new RestClient(BaseUrl);
            var restRequest = new RestRequest(resourceQuery);
            var response = await client.ExecuteGetAsync(restRequest);
            return response;
        }
    }
}
