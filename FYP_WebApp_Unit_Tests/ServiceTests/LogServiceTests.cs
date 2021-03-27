using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Moq;
using Newtonsoft.Json;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class LogServiceTests
    {
        private readonly Mock<IAccessLogRepository> _mockAccessLogRepository;
        private readonly Mock<IApiLogRepository> _mockApiLogRepository;

        public LogServiceTests()
        {
            _mockAccessLogRepository = new Mock<IAccessLogRepository>();
            _mockApiLogRepository = new Mock<IApiLogRepository>();
        }

        [TestMethod]
        public void GetAllApiLogs()
        {
            _mockApiLogRepository.Setup(x => x.GetAll()).Returns(new List<ApiLog>());
            var logService = new LogService(_mockApiLogRepository.Object);
            var result = logService.GetAllApiLogs();
            Assert.AreEqual(typeof(List<ApiLog>), result.GetType());
        }

        [TestMethod]
        public void GetAllAccessLog()
        {
            _mockAccessLogRepository.Setup(x => x.GetAll()).Returns(new List<AccessLog>());
            var logService = new LogService(_mockAccessLogRepository.Object);
            var result = logService.GetAllAccessLogs();
            Assert.AreEqual(typeof(List<AccessLog>),result.GetType());
        }

        [TestMethod]
        public void GetApiLogDetails()
        {
            _mockApiLogRepository.Setup(x => x.GetById(1)).Returns(new ApiLog());
            var logService = new LogService(_mockApiLogRepository.Object);
            var result = logService.GetApiLogDetails(1);
            Assert.AreEqual(typeof(ApiLog), result.GetType());
        }

        [TestMethod]
        public void GetApiLogDetailsNoId()
        {
            _mockApiLogRepository.Setup(x => x.GetById(1)).Returns(new ApiLog());
            var logService = new LogService(_mockApiLogRepository.Object);
            Exception ex = Assert.ThrowsException<ArgumentException>(() => logService.GetApiLogDetails(0));
        }

        [TestMethod]
        public void GetApiLogDetailsNotFound()
        {
            _mockApiLogRepository.Setup(x => x.GetById(1)).Returns(new ApiLog());
            var logService = new LogService(_mockApiLogRepository.Object);
            Exception ex = Assert.ThrowsException<ApiLogNotFoundException>(() => logService.GetApiLogDetails(400));
        }

        [TestMethod]
        public void GetAccessLogDetails()
        {
            _mockAccessLogRepository.Setup(x => x.GetById(1)).Returns(new AccessLog());
            var logService = new LogService(_mockAccessLogRepository.Object);
            var result = logService.GetAccessLogDetails(1);
            Assert.AreEqual(typeof(AccessLog), result.GetType());
        }

        [TestMethod]
        public void GetAccessLogDetailsNoId()
        {
            _mockAccessLogRepository.Setup(x => x.GetById(1)).Returns(new AccessLog());
            var logService = new LogService(_mockAccessLogRepository.Object);
            Exception ex = Assert.ThrowsException<ArgumentException>(() => logService.GetAccessLogDetails(0));
        }

        [TestMethod]
        public void GetAccessLogDetailsNotFound()
        {
            _mockAccessLogRepository.Setup(x => x.GetById(1)).Returns(new AccessLog());
            var logService = new LogService(_mockAccessLogRepository.Object);
            Exception ex = Assert.ThrowsException<AccessLogNotFoundException>(() => logService.GetAccessLogDetails(400));
        }

        [TestMethod]
        public void CreateApiLog()
        {
            var logService = new LogService(_mockApiLogRepository.Object);
            var result = logService.CreateApiLog(new ApiLog());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void CreateAccessLog()
        {
            var logService = new LogService(_mockAccessLogRepository.Object);
            var result = logService.CreateAccessLog(new AccessLog());
            Assert.AreEqual(true, result.Success);
        }

    }
}
