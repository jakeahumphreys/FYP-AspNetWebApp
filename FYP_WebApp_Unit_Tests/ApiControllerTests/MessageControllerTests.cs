using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using FYP_WebApp.API;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp_Unit_Tests.Helpers;
using Moq;

namespace FYP_WebApp_Unit_Tests.ApiControllerTests
{
    [TestClass]
    public class MessageControllerTests
    {
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<IApiLogRepository> _mockApiLogRepository;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<Mapper> _mockMapper;
        private readonly Mock<IPairingRepository> _mockPairingRepository;
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;

        public MessageControllerTests()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockApiLogRepository = new Mock<IApiLogRepository>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            var config = AutomapperConfig.instance().Configure();
            _mockMapper = new Mock<Mapper>(config);
            _mockPairingRepository = new Mock<IPairingRepository>();
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestGet()
        {
            var messageController = new MessageController(_mockMessageRepository.Object, _mockDbContext.Object, _mockApiLogRepository.Object,_mockTeamRepository.Object, _mockMapper.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            var result = messageController.Get();
            Assert.AreEqual(typeof(List<MessageDto>), result.GetType());
        }

        [TestMethod]
        public void TestPostRoutine()
        { 
            var request = new MessageDto
                {MessageType = MessageType.Routine, RecipientId = "2", SenderId = "1", Content = "Test"};

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" }, new ApplicationUser{Id = "2"}});
            var messageController = new MessageController(_mockMessageRepository.Object, _mockDbContext.Object, _mockApiLogRepository.Object, _mockTeamRepository.Object, _mockMapper.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = ApiContextHelper.CreateControllerContext();
            messageController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)messageController.Post(request);
            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
        }

        [TestMethod]
        public void TestPostCheckIn()
        {
            var pairing = new Pairing
                { Id = 1, UserId = "1", BuddyUserId = "2", Start = DateTime.Now, End = DateTime.Now.AddDays(1) };
            var pairings = new List<Pairing> { pairing };

            var request = new MessageDto
                { MessageType = MessageType.CheckIn, RecipientId = "2", SenderId = "1", Content = "Test" };

            _mockPairingRepository.Setup(x => x.GetAll()).Returns(pairings);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" }, new ApplicationUser { Id = "2" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockDbContext.Object, _mockApiLogRepository.Object, _mockTeamRepository.Object, _mockMapper.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = ApiContextHelper.CreateControllerContext();
            messageController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)messageController.Post(request);
            Assert.AreEqual(HttpStatusCode.Accepted, result.StatusCode);
        }

        [TestMethod]
        public void TestPostCheckInNoPairing()
        {
            var pairings = new List<Pairing> { };

            var request = new MessageDto
                { MessageType = MessageType.CheckIn, RecipientId = "2", SenderId = "1", Content = "Test" };

            _mockPairingRepository.Setup(x => x.GetAll()).Returns(pairings);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" }, new ApplicationUser { Id = "2" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockDbContext.Object, _mockApiLogRepository.Object, _mockTeamRepository.Object, _mockMapper.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = ApiContextHelper.CreateControllerContext();
            messageController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)messageController.Post(request);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void TestPostRoutineNoContent()
        {
            var request = new MessageDto
                { MessageType = MessageType.Routine, RecipientId = "2", SenderId = "1", Content = "Test" };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" }, new ApplicationUser { Id = "2" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockDbContext.Object, _mockApiLogRepository.Object, _mockTeamRepository.Object, _mockMapper.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = ApiContextHelper.CreateControllerContext();
            messageController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)messageController.Post(null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

    }
}
