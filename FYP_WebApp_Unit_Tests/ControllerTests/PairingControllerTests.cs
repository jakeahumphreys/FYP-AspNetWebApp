using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public class PairingControllerTests
    {
        private readonly Mock<IPairingRepository> _mockPairingRepository;
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<Library> _mockLibrary;

        public PairingControllerTests()
        {
            _mockPairingRepository = new Mock<IPairingRepository>();
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockLibrary = new Mock<Library>();

        }

        [TestMethod]
        public void TestIndexWithTeam()
        {
            var team = new Team {Id = 1, ManagerId = "1", Name = "Test_Team"};
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser>{new ApplicationUser{Id = "1", TeamId = 1}});
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> {pairing});
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> {team});
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Index();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }


        [TestMethod]
        public void TestIndexWithoutTeam()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> { pairing });
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { team });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Index();
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestIndexWithoutTeamCheckErrorType()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> { pairing });
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { team });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Index();
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestIndexWithoutTeamCheckErrorMessage()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> { pairing });
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { team });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Index();
            Assert.AreEqual("You currently have no team members to pair.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestCreate()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> { pairing });
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { team });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Create();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePost()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            var pairingViewModel = new PairingViewModel
            {
                Pairing = pairing,
                StartDate = new DateTime(2020, 03, 26, 09, 00, 00),
                StartTime = new DateTime(2020, 03, 26, 09, 00, 00),
                EndDate = new DateTime(2020, 03, 27, 15, 00, 00),
                EndTime = new DateTime(2020, 03, 27, 15, 00, 00),
                UserList = null,
                ConflictingPairings = null
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> { pairing });
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { team });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Create(pairingViewModel);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostNoContent()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };

            var pairingViewModel = new PairingViewModel
            {
                Pairing = null,
                StartDate = new DateTime(2020, 03, 26, 09, 00, 00),
                StartTime = new DateTime(2020, 03, 26, 09, 00, 00),
                EndDate = new DateTime(2020, 03, 27, 15, 00, 00),
                EndTime = new DateTime(2020, 03, 27, 15, 00, 00),
                UserList = null,
                ConflictingPairings = null
            };

            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockPairingRepository.Setup(x => x.GetAll()).Returns(new List<Pairing> { pairing });
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { team });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Create(pairingViewModel);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEdit()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Edit(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNoId()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Edit(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNoIdCheckErrorType()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Edit(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }


        [TestMethod]
        public void TestEditNoIdCheckErrorMessage()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Edit(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestEditNotFound()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Edit(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNotFoundCheckErrorType()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Edit(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }


        [TestMethod]
        public void TestEditNotFoundCheckErrorMessage()
        {
            var team = new Team { Id = 1, ManagerId = "2", Name = "Test_Team" };
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Edit(400);
            Assert.AreEqual("A pairing with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDelete()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Delete(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteNoId()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Delete(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteNoIdCheckErrorType()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Delete(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteNoIdCheckErrorMessage()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Delete(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteNotFound()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.Delete(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteNotFoundCheckErrorType()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Delete(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteNotFoundIdCheckErrorMessage()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = (RedirectToRouteResult)pairingController.Delete(400);
            Assert.AreEqual("A pairing with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteConfirmed()
        {
            var pairing = new Pairing
            {
                Id = 1,
                UserId = "1",
                BuddyUserId = "2",
                Start = DateTime.Now,
                End = DateTime.Now.AddDays(1)
            };
            _mockPairingRepository.Setup(x => x.GetById(1)).Returns(pairing);
            var pairingController = new PairingController(_mockPairingRepository.Object, _mockDbContext.Object, _mockTeamRepository.Object, _mockLibrary.Object);
            pairingController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = pairingController.DeleteConfirmed(1);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

    }
}
