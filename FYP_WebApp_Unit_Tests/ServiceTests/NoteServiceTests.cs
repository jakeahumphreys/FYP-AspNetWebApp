using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using FYP_WebApp_Unit_Tests.Helpers;
using Moq;

namespace FYP_WebApp_Unit_Tests.ServiceTests
{
    [TestClass]
    public class NoteServiceTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;

        public NoteServiceTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
        }


        [TestMethod]
        public void TestGetAll()
        {
            _mockNoteRepository.Setup(x => x.GetAll()).Returns(new List<Note>());
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.GetAll();
            Assert.AreEqual(typeof(List<Note>), result.GetType());
        }

        [TestMethod]
        public void TestGetDetails()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.GetDetails(1);
            Assert.AreEqual(typeof(Note), result.GetType());
        }


        [TestMethod]
        public void TestGetDetailsNoId()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteService = new NoteService(_mockNoteRepository.Object);
            Assert.ThrowsException<ArgumentException>(() => noteService.GetDetails(0));
        }

        [TestMethod]
        public void TestGetDetailsNotFound()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteService = new NoteService(_mockNoteRepository.Object);
            Assert.ThrowsException<NoteNotFoundException>(() => noteService.GetDetails(400));
        }

        [TestMethod]
        public void TestCreate()
        {
            var note = new Note {Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now};
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(note);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestCreateNoContent()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(null);
            Assert.AreEqual(false, result.Success);
        }
        
        [TestMethod]
        public void TestCreateNoContentCheckErrorType()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestCreateNoSenderId()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = null, TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(note);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestCreateNoSenderIdCheckErrorType()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = null, TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(note);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestCreateNoTimeCreated()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.MinValue };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(note);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestCreateNoTimeCreatedCheckErrorType()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.MinValue };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Create(note);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestEdit()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Edit(note);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestNoContent()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Edit(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestNoContentCheckErrorType()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(note);
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Edit(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }

        [TestMethod]
        public void TestDelete()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Delete(note);
            Assert.AreEqual(true, result.Success);
        }

        [TestMethod]
        public void TestDeleteNoContent()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Delete(null);
            Assert.AreEqual(false, result.Success);
        }

        [TestMethod]
        public void TestDeleteNoContentCheckErrorType()
        {
            var note = new Note { Id = 1, Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };
            var noteService = new NoteService(_mockNoteRepository.Object);
            var result = noteService.Delete(null);
            Assert.AreEqual(ResponseErrors.NullParameter, result.ResponseError);
        }
    }
}
