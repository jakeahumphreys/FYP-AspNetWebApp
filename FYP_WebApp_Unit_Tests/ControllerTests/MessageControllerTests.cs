using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using FYP_WebApp_Unit_Tests.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class MessageControllerTests
    {
        private readonly Mock<IMessageRepository> _mockMessageRepository;
        private readonly Mock<IPairingRepository> _mockPairingRepository;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;

        public MessageControllerTests()
        {
            _mockMessageRepository = new Mock<IMessageRepository>();
            _mockPairingRepository = new Mock<IPairingRepository>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestViewIndex()
        {
            var message = new Message {Id = 1, RecipientId = "1", Content = "Test"};
            var messages = new List<Message>();
            messages.Add(message);

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> {new ApplicationUser {Id = "1"}});
            _mockMessageRepository.Setup(x => x.GetAll()).Returns(messages);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = messageController.Index(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreateView()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageController.Create(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreateWithRecipient()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageController.Create("1");
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePost()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = messageController.Create("1", "Test");
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostCheckParams()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)messageController.Create("1", "Test");
            Assert.AreEqual(1, result.RouteValues["messageSent"]);
            
        }

        [TestMethod]
        public void TestCreatePostNoRecipient()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = messageController.Create(null, "Test");
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostNoRecipientCheckErrorType()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)messageController.Create(null, "Test");
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestCreatePostNoRecipientCheckErrorMessage()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)messageController.Create(null, "Test");
            Assert.AreEqual("A recipient is required.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestCreatePostNoContent()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = messageController.Create("1", null);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostNoContentCheckErrorType()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)messageController.Create("1", null);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestCreatePostNoContentCheckErrorMessage()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            messageController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)messageController.Create("1", null);
            Assert.AreEqual("A message cannot be empty.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestGetDetailsView()
        {
            var message = new Message {Id = 1, Content = "Test"};
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsViewNoId()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsViewNoIdCheckErrorType()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = (RedirectToRouteResult)messageController.Details(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestGetDetailsViewNoIdCheckErrorMessage()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = (RedirectToRouteResult)messageController.Details(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestGetDetailsViewNotFound()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = messageController.Details(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsViewNotFoundCheckErrorType()
        {
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = (RedirectToRouteResult)messageController.Details(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestGetDetailsViewNotFoundCheckErrorMessage()
        {
            var testId = 400;
            var message = new Message { Id = 1, Content = "Test" };
            _mockMessageRepository.Setup(x => x.GetById(1)).Returns(message);
            var messageController = new MessageController(_mockMessageRepository.Object, _mockPairingRepository.Object,
                _mockTeamRepository.Object, _mockDbContext.Object, _mockConfigRepository.Object);
            var result = (RedirectToRouteResult)messageController.Details(testId);
            Assert.AreEqual($"A message with ID {testId} was not found.", result.RouteValues["Message"]);
        }
    }
}
