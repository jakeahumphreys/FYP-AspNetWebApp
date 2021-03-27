using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Controllers;
using Microsoft.AspNet.Identity;
using Moq;

namespace FYP_WebApp_Unit_Tests.Helpers
{
    class IdentityHelper
    {
        public static Mock<ControllerContext> CreateContextWithAdminUser()
        {
            //Setup User
            var username = "test@test.com";
            var identity = new GenericIdentity(username, "");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, "1");
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.IsInRole("Admin")).Returns(true);
            userMock.Setup(x => x.Identity).Returns(identity);

            //Setup Mock Context
            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(x => x.User).Returns(userMock.Object);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(x => x.HttpContext).Returns(contextMock.Object);
            var controllerMock = new Mock<Controller>();
            

            return controllerContextMock;
        }

        public static Mock<ControllerContext> CreateContextWithManagerUser()
        {
            //Setup User
            var identity = new GenericIdentity("test");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, "1");
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.IsInRole("Manager")).Returns(true);
            userMock.Setup(x => x.Identity).Returns(identity);

            //Setup Mock Context
            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(x => x.User).Returns(userMock.Object);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(x => x.HttpContext).Returns(contextMock.Object);

            return controllerContextMock;
        }

        public static Mock<ControllerContext> CreateContextWithStandardUser()
        {
            //Setup User
            var identity = new GenericIdentity("test");
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, "1");
            identity.AddClaim(nameIdentifierClaim);
            var userMock = new Mock<IPrincipal>();
            userMock.Setup(x => x.IsInRole("Member")).Returns(true);
            userMock.Setup(x => x.Identity).Returns(identity);

            //Setup Mock Context
            var contextMock = new Mock<HttpContextBase>();
            contextMock.SetupGet(x => x.User).Returns(userMock.Object);
            var controllerContextMock = new Mock<ControllerContext>();
            controllerContextMock.SetupGet(x => x.HttpContext).Returns(contextMock.Object);

            return controllerContextMock;
        }

  
    }
}
