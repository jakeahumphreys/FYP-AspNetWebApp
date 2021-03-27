using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using FYP_WebApp_Unit_Tests.Helpers;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class GpsReportControllerTests
    {
        private readonly Mock<IGpsReportRepository> _mockGpsReportRepository;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<IStoredLocationRepository> _mockLocationRepository;
        private readonly Mock<IConfigurationRecordRepository> _configRecordRepository;

        public GpsReportControllerTests()
        {
            _mockGpsReportRepository = new Mock<IGpsReportRepository>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockLocationRepository = new Mock<IStoredLocationRepository>();
            _configRecordRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestIndexAsManager()
        {
            
            //Setup Controller
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            gpsReportController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;

            //Setup Team
            var teamList = new List<Team>();
            var team = new Team {Id = 1, ManagerId = "1"};
            teamList.Add(team);

            _mockTeamRepository.Setup(x => x.GetAll()).Returns(teamList);
            _mockGpsReportRepository.Setup(x => x.GetAll()).Returns(new List<GpsReport>());

            var result = gpsReportController.Index(null, null, null, null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestIndexAsManagerNoTeam()
        {

            //Setup Controller
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            gpsReportController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;

            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team>());
            _mockGpsReportRepository.Setup(x => x.GetAll()).Returns(new List<GpsReport>());

            var result = gpsReportController.Index(null, null, null, null);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestIndexAsAdmin()
        {
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            gpsReportController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;

            _mockGpsReportRepository.Setup(x => x.GetAll()).Returns(new List<GpsReport>());

            var result = gpsReportController.Index(null, null, null, null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetails()
        {
            var gpsReport = new GpsReport {Id = 1};
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(gpsReport);
            _configRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord>{new ConfigurationRecord{Id = 1, MapsApiKey = "Test"}});
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            var result = gpsReportController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoId()
        {
            var gpsReport = new GpsReport { Id = 1 };
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(gpsReport);
            _configRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            var result = gpsReportController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNotFound()
        {
            var gpsReport = new GpsReport { Id = 1 };
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(gpsReport);
            _configRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            var result = gpsReportController.Details(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEdit()
        {
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(new GpsReport());
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object, 
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            var result = gpsReportController.Edit(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNoId()
        {
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(new GpsReport());
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            var result = gpsReportController.Edit(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNotFound()
        {
            _mockGpsReportRepository.Setup(x => x.GetById(1)).Returns(new GpsReport());
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var gpsReportController = new GpsReportController(_mockGpsReportRepository.Object,
                _mockTeamRepository.Object, _mockLocationRepository.Object, _configRecordRepository.Object);
            var result = gpsReportController.Edit(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

    }
}
