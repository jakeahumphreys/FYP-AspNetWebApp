using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp_Unit_Tests.Helpers;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class StoredLocationControllerTests
    {
        private readonly Mock<IStoredLocationRepository> _mockLocationRepository;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<IGpsReportRepository> _mockGpsReportRepository;
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRecordRepository;

        public StoredLocationControllerTests()
        {
            _mockLocationRepository = new Mock<IStoredLocationRepository>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockGpsReportRepository = new Mock<IGpsReportRepository>();
            _mockConfigRecordRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestIndex()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Index(null,null,null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetails()
        {
            var location = new StoredLocation
                {Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0};
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoId()
        {
            var location = new StoredLocation
                { Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0 };
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoIdCheckErrorType()
        {
            var location = new StoredLocation
                { Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0 };
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Details(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDetailsNoIdCheckErrorMessage()
        {
            var location = new StoredLocation
                { Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0 };
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Details(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDetailsNotFound()
        {
            var location = new StoredLocation
            { Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0 };
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Details(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNotFoundCheckErrorType()
        {
            var location = new StoredLocation
            { Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0 };
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Details(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDetailsNotFoundCheckErrorMessage()
        {
            var location = new StoredLocation
            { Id = 1, CheckIns = null, Label = "TestLocation", Latitude = 0, Longitude = 0 };
            _mockConfigRecordRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord> { new ConfigurationRecord { Id = 1, MapsApiKey = "Test" } });
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(location);
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Details(400);
            Assert.AreEqual("Location not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestCreate()
        {
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Create();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }


        [TestMethod]
        public void TestCreatePost()
        {
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Create(new StoredLocation());
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }


        [TestMethod]
        public void TestCreatePostNoContent()
        {
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Create(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreateWithContent()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.CreateWithContent("test", "0", "0");
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestCreateWithContentNoLabel()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.CreateWithContent(null, "0", "0");
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }


        [TestMethod]
        public void TestCreateWithContentNoLabelCheckErrorType()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.CreateWithContent(null, "0", "0");
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestCreateWithContentNoLabelCheckErrorMessage()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.CreateWithContent(null, "0", "0");
            Assert.AreEqual("Unable to create location. Label not provided", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestEditView()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Edit(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNoId()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Edit(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNoIdCheckErrorType()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Edit(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestEditViewNoIdCheckErrorMessage()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Edit(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestEditViewNotFound()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Edit(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNotFoundCheckErrorType()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Edit(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestEditViewNotFoundCheckErrorMessage()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Edit(400);
            Assert.AreEqual("Location not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteView()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Delete(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }


        [TestMethod]
        public void TestDeleteViewNoId()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Delete(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteViewNoIdCheckErrorType()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Delete(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteViewNoIdCheckErrorMessage()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Delete(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteViewNotFound()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.Delete(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteViewNotFoundCheckErrorType()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Delete(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteViewNotFoundCheckErrorMessage()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)locationController.Delete(400);
            Assert.AreEqual("Location not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteAction()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationController = new StoredLocationController(_mockLocationRepository.Object,
                _mockTeamRepository.Object, _mockGpsReportRepository.Object, _mockConfigRecordRepository.Object);
            locationController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = locationController.DeleteAction(1);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }
    }
}
