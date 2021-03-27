using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Moq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class GpsReportServiceTests
    {
        private readonly Mock<IGpsReportRepository> _mockGpsReportRepository;

        public GpsReportServiceTests()
        {
            _mockGpsReportRepository = new Mock<IGpsReportRepository>();
        }

        [TestMethod]
        public void TestGetAll()
        {
            _mockGpsReportRepository.Setup(x => x.GetAll()).Returns(new List<GpsReport>());
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.GetAll();
            Assert.AreEqual(typeof(List<GpsReport>), result.GetType());
        }

        [TestMethod]
        public void TestDetails()
        {
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(new GpsReport());
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.Details(1);
            Assert.AreEqual(typeof(GpsReport), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoId()
        {
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(new GpsReport());
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            Exception ex = Assert.ThrowsException<ArgumentException>(() => gpsReportService.Details(0));

        }

        [TestMethod]
        public void TestDetailsNotFound()
        {
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(new GpsReport());
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            Exception ex = Assert.ThrowsException<GpsReportNotFoundException>(() => gpsReportService.Details(400));
        }

        [TestMethod]
        public void TestCreate()
        {
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.Create(new GpsReport());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateInvalid()
        {
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.Create(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestEdit()
        {
            var gpsReport = new GpsReport {Id = 1};
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(gpsReport);
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.Edit(gpsReport);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestEditInvalid()
        {
            var gpsReport = new GpsReport { Id = 1 };
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(gpsReport);
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.Edit(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestGetGpsReports()
        {
            var users = new List<ApplicationUser>();
            var user = new ApplicationUser {Id = "1"};
            users.Add(user);
            var gpsReports = new List<GpsReport>();
            var gpsReport = new GpsReport {Id = 1, UserId = "1"};
            gpsReports.Add(gpsReport);

            _mockGpsReportRepository.Setup(x => x.GetAll()).Returns(gpsReports);
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            var result = gpsReportService.GetGpsReports(users);
            Assert.AreEqual(typeof(List<GpsReport>), result.GetType());
        }

        [TestMethod]
        public void TestLinkExistingLocations()
        {
            var location = new StoredLocation {Id = 1, Latitude = 1, Longitude = 1};
            var gpsReport = new GpsReport {Id = 2, Longitude = 1, Latitude = 1, LocationId = null};
            var gpsReports = new List<GpsReport>();
            gpsReports.Add(gpsReport);
            _mockGpsReportRepository.Setup(x => x.GetAll()).Returns(gpsReports);
            _mockGpsReportRepository.Setup(x => x.GetById(2)).Returns(gpsReport);
            var gpsReportService = new GpsReportService(_mockGpsReportRepository.Object);
            gpsReportService.LinkExistingLocations(location);
            var result = gpsReportService.Details(2);
            Assert.AreEqual(1, result.LocationId);

        }
    }
}
