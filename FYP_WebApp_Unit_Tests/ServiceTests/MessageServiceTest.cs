using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Moq;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class MessageServiceTest
    {
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly Mock<IPairingRepository> _mockPairingRepository;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;

        public MessageServiceTest()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockPairingRepository = new Mock<IPairingRepository>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestGetAll()
        {
            _mockMessageRepository.Setup(x => x.GetAll()).Returns(new List<Message>());
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.GetAll();
            Assert.AreEqual(typeof(List<Message>), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            var message = new Message {Id = 1, Content = "Test"};
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.GetDetails(1);
            Assert.AreEqual(typeof(Message), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsMatchId()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.GetDetails(1);
            Assert.AreEqual(1, result.Id);
        }

        [TestMethod]
        public void TestGetDetailsMatchContent()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.GetDetails(1);
            Assert.AreEqual("Test", result.Content);
        }

        [TestMethod]
        public void TestGetDetailsNoId()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            Exception ex = Assert.ThrowsException<ArgumentException>(() => messageService.GetDetails(0));
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            Exception ex = Assert.ThrowsException<MessageNotFoundException>(() => messageService.GetDetails(400));
        }

        [TestMethod]
        public void TestCreate()
        {
            var message = new Message { Id = 1, Content = "Test" };
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.Create(message);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateNoMessage()
        {
            var message = new Message { Id = 1, Content = "Test" };
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.Create(null);
            Assert.AreEqual(false, result.Success);
        }


        [TestMethod]
        public void TestCreateNoMessageCheckErrorType()
        {
            var message = new Message { Id = 1, Content = "Test" };
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageService.Create(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestMarkMessageAsRead()
        {
            var message = new Message { Id = 1, Content = "Test", IsRead = false};
            var messageService = new MessageService(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            messageService.MarkMessageAsRead(1);
            var result = messageService.GetDetails(1);
            Assert.AreEqual(true, result.IsRead);
        }

    }
}
