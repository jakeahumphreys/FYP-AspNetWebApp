using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using FYP_WebApp.Controllers;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp_Unit_Tests.Helpers;
using Moq;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class TeamDashboardControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<ITeamRepository> _mockTeamRepository;
        private readonly Mock<IGpsReportRepository> _mockGpsReportRepository;
        private readonly Mock<IPairingRepository> _mockPairingRepository;
        private readonly Mock<IConfigurationRecordRepository> _mockConfigRepository;

        public TeamDashboardControllerTests()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockGpsReportRepository = new Mock<IGpsReportRepository>();
            _mockPairingRepository = new Mock<IPairingRepository>();
            _mockConfigRepository = new Mock<IConfigurationRecordRepository>();
        }

        [TestMethod]
        public void TestIndex()
        {
            var team = new Team {Id = 1, ManagerId = "1", Name = "Team1"};
            var teams = new List<Team>{team};
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(teams);
            var teamDashboardController = new TeamDashboardController(_mockDbContext.Object, _mockTeamRepository.Object,
                _mockGpsReportRepository.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            teamDashboardController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = teamDashboardController.Index();
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestTeamSelect()
        {
            var team = new Team { Id = 1, ManagerId = "1", Name = "Team1" };
            var teams = new List<Team> { team };
            _mockTeamRepository.Setup(x => x.GetAll()).Returns(teams);
            var teamDashboardController = new TeamDashboardController(_mockDbContext.Object, _mockTeamRepository.Object,
                _mockGpsReportRepository.Object, _mockPairingRepository.Object, _mockConfigRepository.Object);
            teamDashboardController.ControllerContext = IdentityHelper.CreateContextWithManagerUser().Object;
            var result = teamDashboardController.TeamSelect();
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }
    }
}
