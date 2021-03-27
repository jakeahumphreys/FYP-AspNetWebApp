using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class ApiLogControllerTests
    {

        private readonly Mock<IApiLogRepository> _mockApiLogRepository;

        public ApiLogControllerTests()
        {
            _mockApiLogRepository = new Mock<IApiLogRepository>();
        }

        [TestMethod]
        public void TestIndex()
        {
            _mockApiLogRepository.Setup(x => x.GetAll()).Returns(new List<ApiLog>());
            var apiLogController = new ApiLogController(_mockApiLogRepository.Object);
            var result = apiLogController.Index(null, null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            _mockApiLogRepository.Setup(x => x.GetById(1)).Returns(new ApiLog());
            var apiLogController = new ApiLogController(_mockApiLogRepository.Object);
            var result = apiLogController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNoId()
        {
            _mockApiLogRepository.Setup(x => x.GetById(1)).Returns(new ApiLog());
            var apiLogController = new ApiLogController(_mockApiLogRepository.Object);
            var result = apiLogController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            _mockApiLogRepository.Setup(x => x.GetById(1)).Returns(new ApiLog());
            var apiLogController = new ApiLogController(_mockApiLogRepository.Object);
            var result = apiLogController.Details(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }
    }
}
