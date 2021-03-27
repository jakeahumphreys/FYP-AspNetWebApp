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
    public class TeamServiceTests
    {
        private Mock<ITeamRepository> _mockTeamRepository;

        public TeamServiceTests()
        {
            _mockTeamRepository = new Mock<ITeamRepository>();
        }

        [TestMethod]
        public void TestGetAll()
        {
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team>());
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.GetAll();
            Assert.AreEqual(typeof(List<Team>), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.GetDetails(1);
            Assert.AreEqual(typeof(Team), result.GetType());
        }

        [TestMethod]
        public void TestGetDetailsNoId()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamService = new TeamService(_mockTeamRepository.Object);
            Assert.ThrowsException<ArgumentException>(() => teamService.GetDetails(0));
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(new Team());
            var teamService = new TeamService(_mockTeamRepository.Object);
            Assert.ThrowsException<TeamNotFoundException>(() => teamService.GetDetails(400));
        }

        [TestMethod]
        public void TestCreate()
        {
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Create(new Team());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateNoContent()
        {
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Create(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestCreateNoContentCheckErrorType()
        {
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Create(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestEdit()
        {
            var team = new Team {Id = 1};
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Edit(team);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestEditNoContent()
        {
            var team = new Team { Id = 1 };
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Edit(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestEditNoContentCheckErrorType()
        {
            var team = new Team { Id = 1 };
            _mockTeamRepository.Setup(x => x.GetById(1)).Returns(team);
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Edit(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestDelete()
        {
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Delete(new Team());
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestDeleteNoContent()
        {
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Delete(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestDeleteNoContentCheckErrorType()
        {
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.Delete(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestGetUserTeams()
        {
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> {new Team {Id = 1, ManagerId = "1"}});
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.GetUserTeams("1");
            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void TestGetUserTeamsUserHasNone()
        {
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(new List<Team> { new Team { Id = 1, ManagerId = "1" } });
            var teamService = new TeamService(_mockTeamRepository.Object);
            var result = teamService.GetUserTeams("2");
            Assert.AreEqual(0, result.Count);
        }
    }
}
