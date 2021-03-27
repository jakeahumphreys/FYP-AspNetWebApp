using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Net;
using System.Web.Http.Controllers;
using System.Web.Http.Results;
using System.Web.Mvc;
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
    public class AccountControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<Mapper> _mockMapper;
        private readonly Mock<IApiLogRepository> _mockApiLogRepository;

        public AccountControllerTests()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            var config = AutomapperConfig.instance().Configure();
            _mockMapper = new Mock<Mapper>(config);
            _mockApiLogRepository = new Mock<IApiLogRepository>();
        }

        [TestMethod]
        public void TestGet()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1" } });
            var accountController = new AccountController(_mockDbContext.Object, _mockApiLogRepository.Object, _mockMapper.Object);
            accountController.ControllerContext = ApiContextHelper.CreateControllerContext();
            accountController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = accountController.Get();
            Assert.AreEqual(typeof(List<MobileUserDto>), result.GetType());
        }

        [TestMethod]
        public void TestLogin()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", MobileLoginKey = "Test", UserName = "Test"} });
            var accountController = new AccountController(_mockDbContext.Object, _mockApiLogRepository.Object, _mockMapper.Object);
            accountController.ControllerContext = ApiContextHelper.CreateControllerContext();
            accountController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = accountController.Login("Test", "Test");
            Assert.AreEqual(typeof(JsonResult<MobileUserDto>), result.GetType());
        }

        [TestMethod]
        public void TestLoginInvalidUsername()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", MobileLoginKey = "Test", UserName = "Test" } });
            var accountController = new AccountController(_mockDbContext.Object, _mockApiLogRepository.Object, _mockMapper.Object);
            accountController.ControllerContext = ApiContextHelper.CreateControllerContext();
            accountController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)accountController.Login("NullTest", "Test");
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void TestLoginInvalidLoginKey()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", MobileLoginKey = "Test", UserName = "Test" } });
            var accountController = new AccountController(_mockDbContext.Object, _mockApiLogRepository.Object, _mockMapper.Object);
            accountController.ControllerContext = ApiContextHelper.CreateControllerContext();
            accountController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)accountController.Login("Test", "InvalidKey");
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void TestLoginInactiveUser()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", MobileLoginKey = "Test", UserName = "Test", IsInactive = true} });
            var accountController = new AccountController(_mockDbContext.Object, _mockApiLogRepository.Object, _mockMapper.Object);
            accountController.ControllerContext = ApiContextHelper.CreateControllerContext();
            accountController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)accountController.Login("Test", "Test");
            Assert.AreEqual(HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [TestMethod]
        public void TestLoginLockedUser()
        {
            _mockDbContext.Setup(x => x.Users).Returns(new MockDbSet<ApplicationUser> { new ApplicationUser { Id = "1", MobileLoginKey = "Test", UserName = "Test", LockoutEndDateUtc = DateTime.Now} });
            var accountController = new AccountController(_mockDbContext.Object, _mockApiLogRepository.Object, _mockMapper.Object);
            accountController.ControllerContext = ApiContextHelper.CreateControllerContext();
            accountController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)accountController.Login("Test", "Test");
            Assert.AreEqual(HttpStatusCode.Forbidden, result.StatusCode);
        }
    }
}
