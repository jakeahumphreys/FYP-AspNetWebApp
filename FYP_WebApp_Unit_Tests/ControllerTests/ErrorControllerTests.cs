using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Controllers;

namespace FYP_WebApp_Unit_Tests.ControllerTests
{
    [TestClass]
    public class ErrorControllerTests
    {
        [TestMethod]
        public void TestIndex()
        {
            var errorController = new ErrorController();
            var result = errorController.Index("test");
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestError()
        {
            var errorController = new ErrorController();
            var result = errorController.Error(Errors.SystemError, "Test");
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }
    }
}
