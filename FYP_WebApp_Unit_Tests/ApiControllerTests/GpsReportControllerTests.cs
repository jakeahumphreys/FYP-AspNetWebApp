using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using FYP_WebApp.API;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using Moq;

namespace FYP_WebApp_Unit_Tests.ApiControllerTests
{
    [TestClass]
    public class GpsReportControllerTests
    {
        private readonly Mock<IGpsReportRepository> _gpsReportRepository;
        private readonly Mock<IStoredLocationRepository> _mockLocationRepository;
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;
        private readonly Mock<IApiLogRepository> _mockApiLogRepository;
        private readonly Mock<Mapper> _mockMapper;

        public GpsReportControllerTests()
        {
            _gpsReportRepository = new Mock<IGpsReportRepository>();
            _mockLocationRepository = new Mock<IStoredLocationRepository>();
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
            _mockApiLogRepository = new Mock<IApiLogRepository>();
            var config = AutomapperConfig.instance().Configure();
            _mockMapper = new Mock<Mapper>(config);
        }

        [TestMethod]
        public void TestPost()
        {
            var request = new GpsReportDto
            {
                Latitude = 0,
                Longitude = 0,
                Time = DateTime.Now,
                UserId = "1"
            };

            _mockConfigRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord>
                {new ConfigurationRecord {Id = 1, StoreLocationAccuracy = 25}});
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>
                {new StoredLocation {Id = 1, Latitude = 0, Longitude = 0, Label = "Test"}});
            var gpsReportController = new GpsReportController(_gpsReportRepository.Object,
                _mockLocationRepository.Object, _mockConfigRepository.Object, _mockApiLogRepository.Object,
                _mockMapper.Object);
            gpsReportController.ControllerContext = ApiContextHelper.CreateControllerContext();
            gpsReportController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)gpsReportController.Post(request);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void TestPostNoContent()
        {
            var request = new GpsReportDto
            {
                Latitude = 0,
                Longitude = 0,
                Time = DateTime.Now,
                UserId = "1"
            };

            _mockConfigRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord>
                {new ConfigurationRecord {Id = 1, StoreLocationAccuracy = 25}});
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>
                {new StoredLocation {Id = 1, Latitude = 0, Longitude = 0, Label = "Test"}});
            var gpsReportController = new GpsReportController(_gpsReportRepository.Object,
                _mockLocationRepository.Object, _mockConfigRepository.Object, _mockApiLogRepository.Object,
                _mockMapper.Object);
            gpsReportController.ControllerContext = ApiContextHelper.CreateControllerContext();
            gpsReportController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)gpsReportController.Post(null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
