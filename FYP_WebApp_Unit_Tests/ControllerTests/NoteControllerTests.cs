using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
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
    public class NoteControllerTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;

        public NoteControllerTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
        }

        [TestMethod]
        public void TestDetails()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Details(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoId()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Details(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNoIdCheckErrorType()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Details(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDetailsNoIdCheckErrorMessage()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Details(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDetailsNotFound()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Details(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDetailsNotFoundCheckErrorType()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Details(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDetailsNotFoundCheckErrorMessage()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Details(400);
            Assert.AreEqual("A Note with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestCreateView()
        {
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Create(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePost()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1 };
            var noteController = new NoteController(_mockNoteRepository.Object);
            noteController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = noteController.Create(new NoteViewModel { Note = note });
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestCreatePostSuccessParams()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1 };
            var noteController = new NoteController(_mockNoteRepository.Object);
            noteController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)noteController.Create(new NoteViewModel { Note = note, StoredLocationId = 1 });
            Assert.AreEqual(1, result.RouteValues["id"]);
        }


        [TestMethod]
        public void TestCreatePostNoContent()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1 };
            var noteController = new NoteController(_mockNoteRepository.Object);
            noteController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = noteController.Create(new NoteViewModel { Note = null });
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }


        [TestMethod]
        public void TestCreatePostNoContentCheckErrorType()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1 };
            var noteController = new NoteController(_mockNoteRepository.Object);
            noteController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)noteController.Create(new NoteViewModel { Note = null });
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }


        [TestMethod]
        public void TestCreatePostNoContentCheckErrorMessage()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1 };
            var noteController = new NoteController(_mockNoteRepository.Object);
            noteController.ControllerContext = IdentityHelper.CreateContextWithAdminUser().Object;
            var result = (RedirectToRouteResult)noteController.Create(new NoteViewModel { Note = null });
            Assert.AreEqual("Invalid Parameter", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestEdit()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Edit(note);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestEditNoContent()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Edit(null);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDelete()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Delete(1);
            Assert.AreEqual(typeof(ViewResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteNoId()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Delete(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteNoIdCheckErrorType()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteNoIdCheckErrorMessage()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteNotFound()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Delete(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteNotFoundCheckErrorType()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteNotFoundCheckErrorMessage()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(400);
            Assert.AreEqual("A Note with ID 400 was not found.", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteConfirmedNoId()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Delete(0);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteConfirmedCheckErrorType()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(0);
            Assert.AreEqual(Errors.InvalidParameter, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteConfirmedNoIdCheckErrorMessage()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(0);
            Assert.AreEqual("No ID specified", result.RouteValues["Message"]);
        }

        [TestMethod]
        public void TestDeleteConfirmedNotFound()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = noteController.Delete(400);
            Assert.AreEqual(typeof(RedirectToRouteResult), result.GetType());
        }

        [TestMethod]
        public void TestDeleteConfirmedNotFoundCheckErrorType()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(400);
            Assert.AreEqual(Errors.EntityNotFound, result.RouteValues["Error"]);
        }

        [TestMethod]
        public void TestDeleteConfirmedNotFoundCheckErrorMessage()
        {
            var note = new Note { Id = 1, SenderId = "1", TimeCreated = DateTime.Now, StoredLocationId = 1, Content = "Test" };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteController = new NoteController(_mockNoteRepository.Object);
            var result = (RedirectToRouteResult)noteController.Delete(400);
            Assert.AreEqual("A Note with ID 400 was not found.", result.RouteValues["Message"]);
        }
    }
}
