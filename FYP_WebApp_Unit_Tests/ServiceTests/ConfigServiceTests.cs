using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Moq;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class ConfigServiceTests
    {
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;

        public ConfigServiceTests()
        {
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestGetAll()
        {
            _mockConfigRepository.Setup(x => x.GetAll()).Returns(new List<ConfigurationRecord>());
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var result = configService.GetAll();
            Assert.AreEqual(typeof(List<ConfigurationRecord>), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var result = configService.GetDetails(1);
            Assert.AreEqual(typeof(ConfigurationRecord), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNoId()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            Exception ex = Assert.ThrowsException<ArgumentException>(() => configService.GetDetails(0));
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(new ConfigurationRecord());
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            Exception ex = Assert.ThrowsException<ConfigurationRecordNotFoundException>(() => configService.GetDetails(400));
        }

        [TestMethod]
        public void TestCreate()
        {
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var result = configService.Create(new ConfigurationRecord());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateInvalid()
        {
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var result = configService.Create(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestEdit()
        {
            var testData = new ConfigurationRecord {Id = 1, Created = DateTime.Now};
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(testData);
            var result = configService.Edit(testData);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestEditInvalid()
        {
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var result = configService.Edit(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestDelete()
        {
            var testData = new ConfigurationRecord { Id = 1, Created = DateTime.Now };
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            _mockConfigRepository.Setup(x => x.GetById(1)).Returns(testData);
            var result = configService.Delete(testData);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestDeleteInvalid()
        {
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var result = configService.Delete(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestGetLatestConfigRecord()
        {
            var configService = new ConfigurationRecordService(_mockConfigRepository.Object);
            var testDataList = new List<ConfigurationRecord>();
            testDataList.Add(new ConfigurationRecord{Id = 1, Created = new DateTime(2020, 10, 02)});
            testDataList.Add(new ConfigurationRecord { Id = 2, Created = new DateTime(2021, 03, 10)});
            _mockConfigRepository.Setup(x => x.GetAll()).Returns(testDataList);
            var result = configService.GetLatestConfigurationRecord();
            Assert.AreEqual(2, result.Id);
        }
    }
}
