using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Moq;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class StoredLocationServiceTests
    {
        private Mock<IStoredLocationRepository> _mockLocationRepository;

        public StoredLocationServiceTests()
        {
            _mockLocationRepository = new Mock<IStoredLocationRepository>();
        }

        [TestMethod]
        public void TestIndex()
        {
            _mockLocationRepository.Setup(x => x.GetAll()).Returns(new List<StoredLocation>());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.Index();
            Assert.AreEqual(typeof(List<StoredLocation>), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.GetDetails(1);
            Assert.AreEqual(typeof(StoredLocation), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNoId()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            Assert.ThrowsException<ArgumentException>(() => locationService.GetDetails(0));
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            Assert.ThrowsException<StoredLocationNotFoundException>(() => locationService.GetDetails(400));
        }

        [TestMethod]
        public void TestCreateAction()
        {
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.CreateAction(new StoredLocation());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateActionNoContent()
        {
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.CreateAction(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestEditView()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.EditView(1);
            Assert.AreEqual(typeof(StoredLocation), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNoId()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            Assert.ThrowsException<ArgumentException>(() => locationService.EditView(0));
        }

        [TestMethod]
        public void TestEditViewNotFound()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            Assert.ThrowsException<StoredLocationNotFoundException>(() => locationService.EditView(400));
        }

        [TestMethod]
        public void TestEditAction()
        {
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.EditAction(new StoredLocation());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestEditActionNoContent()
        {
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.EditAction(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestDeleteView()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.DeleteView(1);
            Assert.AreEqual(typeof(StoredLocation), result.GetType());
        }

        [TestMethod]
        public void TestDeleteViewNoId()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            Assert.ThrowsException<ArgumentException>(() => locationService.DeleteView(0));
        }

        [TestMethod]
        public void TestDeleteViewNotFound()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            Assert.ThrowsException<StoredLocationNotFoundException>(() => locationService.DeleteView(400));
        }

        [TestMethod]
        public void TestDeleteAction()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.DeleteAction(1);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestDeleteActionNoContent()
        {
            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation());
            var locationService = new StoredLocationService(_mockLocationRepository.Object);
            var result = locationService.DeleteAction(0);
            Assert.AreEqual(false, result.Success);
        }
    }
}
