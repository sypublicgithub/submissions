using Duplocloud.API.Configurations;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace duplocloud.tests
{
    [TestClass]
    public class ConfigurationTests
    {
        [TestMethod]
        public void WeatherForecastUrl_initializes_to_null()
        {
            var mockOpenMeteoConfig = new OpenMeteoConfig();
            mockOpenMeteoConfig.WeatherForecastUrl = "mock_WeatherForecastUrl";
            mockOpenMeteoConfig.CurrentForecastQueryParams = "mock_CurrentForecastQueryParams";

            var actual = Options.Create(mockOpenMeteoConfig);

            Assert.IsInstanceOfType(actual.Value, typeof(OpenMeteoConfig));
            Assert.AreSame(actual.Value.WeatherForecastUrl, mockOpenMeteoConfig.WeatherForecastUrl);
            Assert.AreSame(actual.Value.CurrentForecastQueryParams, mockOpenMeteoConfig.CurrentForecastQueryParams);
        }
    }
}