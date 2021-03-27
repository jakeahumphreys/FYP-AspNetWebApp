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
    public class StoredLocationControllerTests
    {
        private Mock<IStoredLocationRepository> _mockLocationRepository;
        private Mock<IApiLogRepository> _mockApiLogRepository;
        private Mock<Mapper> _mockMapper;

        public StoredLocationControllerTests()
        {
            _mockLocationRepository = new Mock<IStoredLocationRepository>();
            _mockApiLogRepository = new Mock<IApiLogRepository>();
            var config = AutomapperConfig.instance().Configure();
            _mockMapper = new Mock<Mapper>(config);
        }

        [TestMethod]
        public void TestGet()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockApiLogRepository.Object, _mockMapper.Object);
            locationController.ControllerContext = ApiContextHelper.CreateControllerContext();
            locationController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = locationController.Get();
            Assert.AreEqual(typeof(List<StoredLocationDto>), result.GetType());
        }

        [TestMethod]
        public void TestGetById()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockApiLogRepository.Object, _mockMapper.Object);
            locationController.ControllerContext = ApiContextHelper.CreateControllerContext();
            locationController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = locationController.Get(1);
            Assert.AreEqual(typeof(JsonResult<StoredLocationDto>), result.GetType());
        }

        [TestMethod]
        public void TestGetByIdNoId()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockApiLogRepository.Object, _mockMapper.Object);
            locationController.ControllerContext = ApiContextHelper.CreateControllerContext();
            locationController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)locationController.Get(0);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }


        [TestMethod]
        [ExpectedException(typeof(StoredLocationNotFoundException))]
        public void TestGetByIdNotFound()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockApiLogRepository.Object, _mockMapper.Object);
            locationController.ControllerContext = ApiContextHelper.CreateControllerContext();
            locationController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)locationController.Get(400);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }
    }
}
