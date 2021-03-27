using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Runtime.CompilerServices;
using System.Web.Http.Results;
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
    public class StatusControllerTests
    {
        private readonly Mock<ApplicationDbContext> _mockDbContext;
        private readonly Mock<IApiLogRepository> _apiLogRepository;

        public StatusControllerTests()
        {
            _mockDbContext = new Mock<ApplicationDbContext>();
            _apiLogRepository = new Mock<IApiLogRepository>();
        }

        [TestMethod]
        public void TestInvalidPost()
        {
            var request = new StatusDto {status = Status.AtLocation, UserId = "1"};
            _mockDbContext.Setup(x => x.Users).Returns(new MockUserDbSet());
            var statusController = new StatusController(_mockDbContext.Object, _apiLogRepository.Object);
            statusController.ControllerContext = ApiContextHelper.CreateControllerContext();
            statusController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)statusController.Post(null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
