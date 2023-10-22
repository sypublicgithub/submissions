using AutoMapper;
using duplocloud.Controllers;
using Duplocloud.API.Configurations;
using Duplocloud.API.Datastore;
using Duplocloud.API.Datastore.DBModels;
using Duplocloud.API.Models;
using Duplocloud.API.Services;
using Duplocloud.API.Services.OpenMeteo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Duplocloud.API.Tests.Controllers
{
    [TestClass]
    public class ControllerTests
    {
        private Mock<IWeatherForecastResolver> MockWeatherForecastResolver;
        private Mock<IWeatherForecastRepository> MockWeatherForecastRepository;

        [TestInitialize]
        public void Setup()
        {
            this.MockWeatherForecastResolver = new Mock<IWeatherForecastResolver>();
            this.MockWeatherForecastRepository = new Mock<IWeatherForecastRepository>();
        }

        [TestMethod]
        public async Task WeatherForecastController_get_returns_forecast()
        {
            var mockWeatherForecast = new Mock<WeatherForecast>();
            var mockSavedWeatherForecast = new Mock<WeatherForecast>();

            this.MockWeatherForecastResolver.Setup(x => x.GetWeatherForecast(It.IsAny<WeatherForecastRequest>()))
                .ReturnsAsync(mockWeatherForecast.Object);

            this.MockWeatherForecastRepository.Setup(x => x.SaveWeatherForecast(mockWeatherForecast.Object))
                .ReturnsAsync(mockSavedWeatherForecast.Object);

            var actual = new WeatherForecastController(
                this.MockWeatherForecastResolver.Object,
                this.MockWeatherForecastRepository.Object);

            var result = await actual.Get(10, 12);

            this.MockWeatherForecastRepository.Verify(x => x.SaveWeatherForecast(mockWeatherForecast.Object), Times.Once);
            this.MockWeatherForecastResolver.Verify(x => x.GetWeatherForecast(It.IsAny<WeatherForecastRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task WeatherForecastController_post_returns_forecast()
        {
            var mockWeatherForecast = new Mock<WeatherForecast>();
            var mockSavedWeatherForecast = new Mock<WeatherForecast>();

            this.MockWeatherForecastResolver.Setup(x => x.GetWeatherForecast(It.IsAny<WeatherForecastRequest>()))
                .ReturnsAsync(mockWeatherForecast.Object);

            this.MockWeatherForecastRepository.Setup(x => x.SaveWeatherForecast(mockWeatherForecast.Object))
                .ReturnsAsync(mockSavedWeatherForecast.Object);

            var actual = new WeatherForecastController(
                this.MockWeatherForecastResolver.Object,
                this.MockWeatherForecastRepository.Object);

            var result = await actual.Post(new WeatherForecastRequest
            {
                latitude = 10,
                longitude = 12
            });

            this.MockWeatherForecastRepository.Verify(x => x.SaveWeatherForecast(mockWeatherForecast.Object), Times.Once);
            this.MockWeatherForecastResolver.Verify(x => x.GetWeatherForecast(It.IsAny<WeatherForecastRequest>()), Times.Once);
        }

        [TestMethod]
        public async Task WeatherForecastController_GetLongitudeLatitudeHistory_returns_forecasts_requests()
        {
            var mockWeatherForecastRequest1 = new Mock<WeatherForecastRequest>();
            var mockWeatherForecastRequest2 = new Mock<WeatherForecastRequest>();

            this.MockWeatherForecastRepository.Setup(x => x.GetAllLongitudeLatitudeHistory())
                .ReturnsAsync(new[] { mockWeatherForecastRequest1.Object, mockWeatherForecastRequest2.Object });

            var actual = new WeatherForecastController(
                this.MockWeatherForecastResolver.Object,
                this.MockWeatherForecastRepository.Object);

            var result = await actual.GetLongitudeLatitudeHistory();

            this.MockWeatherForecastRepository.Verify(x => x.GetAllLongitudeLatitudeHistory(), Times.Once);
            Assert.IsTrue(2 == result.Length);
        }

        [TestMethod]
        public async Task WeatherForecastController_GetWeatherForecastHistory_returns_forecasts()
        {
            var mockWeatherForecast1 = new Mock<WeatherForecast>();
            var mockWeatherForecast2 = new Mock<WeatherForecast>();

            this.MockWeatherForecastRepository.Setup(x => x.GetWeatherForecastHistory())
                .ReturnsAsync(new[] { mockWeatherForecast1.Object, mockWeatherForecast2.Object });

            var actual = new WeatherForecastController(
                this.MockWeatherForecastResolver.Object,
                this.MockWeatherForecastRepository.Object);

            var result = await actual.GetWeatherForecastHistory();

            this.MockWeatherForecastRepository.Verify(x => x.GetWeatherForecastHistory(), Times.Once);
            Assert.IsTrue(2 == result.Length);
        }

        [TestMethod]
        public async Task WeatherForecastController_Delete_returns_OK_if_successfuly_deleted()
        {
            this.MockWeatherForecastRepository.Setup(x => x.DeleteWeatherForecast(It.IsAny<Guid>()))
                .ReturnsAsync(true);

            var actual = new WeatherForecastController(
                this.MockWeatherForecastResolver.Object,
                this.MockWeatherForecastRepository.Object);

            var result = await actual.Delete(Guid.Empty);

            this.MockWeatherForecastRepository.Verify(x => x.DeleteWeatherForecast(It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.OK);
        }

        [TestMethod]
        public async Task WeatherForecastController_Delete_returns_NotModified_is_delete_fails()
        {
            this.MockWeatherForecastRepository.Setup(x => x.DeleteWeatherForecast(It.IsAny<Guid>()))
                .ReturnsAsync(false);

            var actual = new WeatherForecastController(
                this.MockWeatherForecastResolver.Object,
                this.MockWeatherForecastRepository.Object);

            var result = await actual.Delete(Guid.Empty);

            this.MockWeatherForecastRepository.Verify(x => x.DeleteWeatherForecast(It.IsAny<Guid>()), Times.Once);
            Assert.IsTrue(result.StatusCode == (int)HttpStatusCode.NotModified);
        }
    }
}
