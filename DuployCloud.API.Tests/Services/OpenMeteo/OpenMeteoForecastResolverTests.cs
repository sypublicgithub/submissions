using AutoMapper;
using Duplocloud.API.Configurations;
using Duplocloud.API.Models;
using Duplocloud.API.Services;
using Duplocloud.API.Services.OpenMeteo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RestSharp;

namespace Duplocloud.API.Tests.Services.OpenMeteo
{
    [TestClass]
    public class OpenMeteoForecastResolverTests
    {
        private Mock<OpenMeteoConfig> MockOpenMeteoConfig;
        private Mock<IOptions<OpenMeteoConfig>> MockOpenMeteoConfigOption;
        private Mock<ILogger<OpenMeteoForecastResolver>> MockLogger;
        private Mock<IMapper> MockMapper;

        [TestInitialize]
        public void Setup()
        {
            this.MockOpenMeteoConfig = new Mock<OpenMeteoConfig>();
            this.MockOpenMeteoConfigOption = new Mock<IOptions<OpenMeteoConfig>>();
            this.MockLogger = new Mock<ILogger<OpenMeteoForecastResolver>>();
            this.MockMapper = new Mock<IMapper>();
        }

        [TestMethod]
        public void OpenMeteoForecastResolver_implements_IWeatherForecastResolver()
        {
            this.MockOpenMeteoConfigOption.Setup(x => x.Value).Returns(this.MockOpenMeteoConfig.Object);
            var actual = new OpenMeteoForecastResolver(
                this.MockOpenMeteoConfigOption.Object,
                this.MockLogger.Object,
                this.MockMapper.Object);

            Assert.IsTrue(actual is IWeatherForecastResolver == true);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "IOptions", AllowDerivedTypes = false)]
        public void constructor_throws_exception_if_injected_IOptions_instance_is_null()
        {
            new OpenMeteoForecastResolver(
                this.MockOpenMeteoConfigOption.Object,
                this.MockLogger.Object,
                this.MockMapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "ILogger", AllowDerivedTypes = false)]
        public void constructor_throws_exception_if_injected_instance_is_null()
        {
            var mockOpenMeteoConfig = new Mock<OpenMeteoConfig>();
            this.MockOpenMeteoConfigOption.Setup(x => x.Value).Returns(this.MockOpenMeteoConfig.Object);

            var actual = new OpenMeteoForecastResolver(
                this.MockOpenMeteoConfigOption.Object,
                null,
                this.MockMapper.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "IMapper", AllowDerivedTypes = false)]
        public void constructor_throws_exception_if_injected_IMapper_instance_is_null()
        {
            var mockOpenMeteoConfig = new Mock<OpenMeteoConfig>();
            this.MockOpenMeteoConfigOption.Setup(x => x.Value).Returns(this.MockOpenMeteoConfig.Object);

            var actual = new OpenMeteoForecastResolver(
                this.MockOpenMeteoConfigOption.Object,
                this.MockLogger.Object,
                null);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidRequestException))]
        public async Task GetWeatherForecast_throws_invalid_exception_is_json_is_return_but_is_invalid()
        {
            this.MockOpenMeteoConfig.Setup(x => x.WeatherForecastUrl).Returns("https://mock.com");
            this.MockOpenMeteoConfig.Setup(x => x.CurrentForecastQueryParams).Returns("mock_CurrentForecastQueryParams");
            this.MockOpenMeteoConfigOption.Setup(x => x.Value).Returns(this.MockOpenMeteoConfig.Object);

            var mockWeatherForecast = new Mock<WeatherForecast>();
            this.MockMapper.Setup(x => x.Map<WeatherForecast>(It.IsAny<ForecastSummary>())).Returns(mockWeatherForecast.Object);

            var actual = new OpenMeteoForecastResolverStub(
                this.MockOpenMeteoConfigOption.Object,
                this.MockLogger.Object,
                this.MockMapper.Object);

            actual.MockSuccess = false;
            var result = await actual.GetWeatherForecast(new Models.WeatherForecastRequest()
            {
                latitude = 100,
                longitude = 100
            });
        }

        [TestMethod]
        public async Task GetWeatherForecast_returns_forecast_when_response_is_valid()
        {
            this.MockOpenMeteoConfig.Setup(x => x.WeatherForecastUrl).Returns("https://mock.com");
            this.MockOpenMeteoConfig.Setup(x => x.CurrentForecastQueryParams).Returns("mock_CurrentForecastQueryParams");
            this.MockOpenMeteoConfigOption.Setup(x => x.Value).Returns(this.MockOpenMeteoConfig.Object);

            var mockWeatherForecast = new Mock<WeatherForecast>();
            this.MockMapper.Setup(x => x.Map<WeatherForecast>(It.IsAny<ForecastSummary>())).Returns(mockWeatherForecast.Object);

            var actual = new OpenMeteoForecastResolverStub(
                this.MockOpenMeteoConfigOption.Object,
                this.MockLogger.Object,
                this.MockMapper.Object);
            
            var result = await actual.GetWeatherForecast(new Models.WeatherForecastRequest()
            {
                latitude = 100,
                longitude = 100
            });

            Assert.IsInstanceOfType(result, typeof(WeatherForecast));
            this.MockOpenMeteoConfig.Verify(x => x.WeatherForecastUrl, Times.AtLeastOnce);
            this.MockOpenMeteoConfig.Verify(x => x.CurrentForecastQueryParams, Times.Once);
        }
    }
}
