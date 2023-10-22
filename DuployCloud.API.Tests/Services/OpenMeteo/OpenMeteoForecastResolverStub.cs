using AutoMapper;
using Duplocloud.API.Configurations;
using Duplocloud.API.Models;
using Duplocloud.API.Services.OpenMeteo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using RestSharp;

namespace Duplocloud.API.Tests.Services.OpenMeteo
{
    public class OpenMeteoForecastResolverStub : OpenMeteoForecastResolver
    {
        public bool MockSuccess { get; set; } = true;
        public OpenMeteoForecastResolverStub(
            IOptions<OpenMeteoConfig> OpenMeteoConfig,
            ILogger<OpenMeteoForecastResolver> logger,
            IMapper mapper) : base(OpenMeteoConfig, logger, mapper)
        {
        }

        /// <summary>
        /// test stub
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected override async Task<RestResponse> CallRestClient(string resourceQuery)
        {
            var mockResponse = new Mock<RestResponse>();
            var mockJson = "{}";
            mockResponse.Object.Content = mockJson;
            if (MockSuccess)
            {
                mockResponse.Object.IsSuccessStatusCode = true;
                mockResponse.Object.ResponseStatus = ResponseStatus.Completed;
            }

            return mockResponse.Object;
        }
    }
}
