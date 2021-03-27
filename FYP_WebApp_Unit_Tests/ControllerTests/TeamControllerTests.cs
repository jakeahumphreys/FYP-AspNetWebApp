using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp_Unit_Tests.Helpers;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class TeamControllerTests
    {
        private Mock<ITeamRepository> _mockTeamRepository;
        private Mock<ApplicationDbContext> _mockDbContext;

        public TeamControllerTests()
        {
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockDbContext = new Mock<ApplicationDbContext>();
        }

        [TestMethod]
        public void TestIndex()
        {
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team>());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Index();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetails()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoId()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoIdCheckErrorType()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Details(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDetailsNoIdCheckErrorMessage()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Details(0);
            Assert.AreEqual("No ID specified.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDetailsNotFound()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Details(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNotFoundCheckErrorType()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Details(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDetailsNotFoundCheckErrorMessage()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Details(400);
            Assert.AreEqual("A team with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestCreateView()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Create();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePost()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> {new IdentityRole("Manager")});
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Create(new Team());
            Assert.AreEqual(typeof(RedirectResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostNoContent()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Create(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }


        [TestMethod]
        public void TestEditView()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Edit(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNoId()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Edit(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNoIdCheckErrorType()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Edit(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestEditViewNoIdCheckErrorMessage()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Edit(0);
            Assert.AreEqual("No ID specified.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestEditViewNotFound()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Edit(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditViewNotFoundCheckErrorType()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Edit(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestEditViewNotFoundCheckErrorMessage()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", TeamId = 1 } });
            _mockDbContext.Setup(x => x.Roles).Returns(new MockDbSet<IdentityRole> { new IdentityRole("Manager") });
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Edit(400);
            Assert.AreEqual("A team with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestEditPost()
        {
            var team = new Team {Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null};
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Edit(team);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditPostNoContent()
        {
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Edit(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteView()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Delete(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteViewNoId()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Delete(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteViewNoIdCheckErrorType()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Delete(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteViewNoIdCheckErrorMessage()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Delete(0);
            Assert.AreEqual("No ID specified.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteViewNotFound()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.Delete(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteViewNotFoundCheckErrorType()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Delete(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteViewNotFoundCheckErrorMessage()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = (RedirectToRouteResult)teamController.Delete(400);
            Assert.AreEqual("A team with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteConfirmed()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Test_Team", TeamMembers = null };
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var teamController = new TeamController(_mockTeamRepository.Object, _mockDbContext.Object);
            var result = teamController.DeleteConfirmed(1);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

    }
}
