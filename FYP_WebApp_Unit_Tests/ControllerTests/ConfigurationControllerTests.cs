using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Web.Mvc;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class ConfigurationControllerTests
    {
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;

        public ConfigurationControllerTests()
        {
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestIndex()
        {
            _mockConfigRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord>());
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Index();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetails()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreate()
        {
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Create();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostValid()
        {
            var testConfig = new ConfigurationRecord();
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Create(testConfig);
            Assert.AreEqual(typeof(RedirectResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostInvalid()
        {
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Create(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestEdit()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Edit(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNoId()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Edit(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNotFound()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            var result = configController.Edit(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditPostValid()
        {
            var testRecord = new ConfigurationRecord {Id = 1, Created= DateTime.Now};
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(testRecord);
            var result = configController.Edit(testRecord);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditPostInvalid()
        {
            var testRecord = new ConfigurationRecord { Id = 1, Created = DateTime.Now };
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(testRecord);
            var result = configController.Edit(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDelete()
        {
            var testRecord = new ConfigurationRecord { Id = 1, Created = DateTime.Now };
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(testRecord);
            var result = configController.Delete(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteConfirmed()
        {
            var testRecord = new ConfigurationRecord { Id = 1, Created = DateTime.Now };
            var configController = new ConfigurationRecordController(_mockConfigRepository.Object);
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(testRecord);
            var result = configController.DeleteConfirmed(1);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }
    }
}
