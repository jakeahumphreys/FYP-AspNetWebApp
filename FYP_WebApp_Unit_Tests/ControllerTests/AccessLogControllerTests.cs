using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class AccessLogControllerTests
    {
        private readonly Mock<IAccessLogRepository> _mockAccessLogRepository;

        public AccessLogControllerTests()
        {
            _mockAccessLogRepository = new Mock<IAccessLogRepository>();
        }

        [TestMethod]
        public void TestIndex()
        {
            _mockAccessLogRepository.Setup(x => x.GetAll()).Returns(new List<AccessLog>());
            var accessLogController = new AccessLogController(_mockAccessLogRepository.Object);
            var result = accessLogController.Index(null,null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            _mockAccessLogRepository.Setup(x => x.GetById(1)).Returns(new AccessLog());
            var accessLogController = new AccessLogController(_mockAccessLogRepository.Object);
            var result = accessLogController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNoId()
        {
            _mockAccessLogRepository.Setup(x => x.GetById(1)).Returns(new AccessLog());
            var accessLogController = new AccessLogController(_mockAccessLogRepository.Object);
            var result = accessLogController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            _mockAccessLogRepository.Setup(x => x.GetById(1)).Returns(new AccessLog());
            var accessLogController = new AccessLogController(_mockAccessLogRepository.Object);
            var result = accessLogController.Details(2);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }
    }
}
