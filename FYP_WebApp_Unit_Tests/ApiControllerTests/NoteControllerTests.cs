using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Http.Results;
using AutoMapper;
using FYP_WebApp.API;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using Moq;

namespace FYP_WebApp_Unit_Tests.ApiControllerTests
{
    [TestClass]
    public class NoteControllerTests
    {
        private readonly Mock<INoteRepository> _mockNoteRepository;
        private readonly Mock<IApiLogRepository> _mockApiLogRepository;
        private readonly Mock<IStoredLocationRepository> _mockLocationRepository;
        private readonly Mock<Mapper> _mockMapper;

        public NoteControllerTests()
        {
            _mockNoteRepository = new Mock<INoteRepository>();
            _mockApiLogRepository = new Mock<IApiLogRepository>();
            _mockLocationRepository = new Mock<IStoredLocationRepository>();
            var config = AutomapperConfig.instance().Configure();
            _mockMapper = new Mock<Mapper>(config);
        }

        [TestMethod]
        public void TestGet()
        {
            _mockNoteRepository.Setup(x => x.GetAll()).Returns(new List<Note>());
            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = noteController.Get();
            Assert.AreEqual(typeof(List<NoteDto>), result.GetType());
        }

        [TestMethod]
        public void TestGetWithId()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = noteController.Get(1);
            Assert.AreEqual(typeof(JsonResult<NoteDto>), result.GetType());
        }

        [TestMethod]
        public void TestGetWithIdNoId()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)noteController.Get(0);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void TestGetWithIdNotFound()
        {
            _mockNoteRepository.Setup(x => x.GetById(1)).Returns(new Note());
            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)noteController.Get(400);
            Assert.AreEqual(HttpStatusCode.NotFound, result.StatusCode);
        }

        [TestMethod]
        public void TestPost()
        {
            var request = new NoteDto {Content = "Test", SenderId = "1", TimeCreated = DateTime.Now};

            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation {Id = 1, Label = "TestLoc"});

            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)noteController.Post(1, request);
            Assert.AreEqual(HttpStatusCode.OK, result.StatusCode);
        }

        [TestMethod]
        public void TestPostNoLocationId()
        {
            var request = new NoteDto { Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };

            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation { Id = 1, Label = "TestLoc" });

            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)noteController.Post(0, request);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [TestMethod]
        public void TestPostNoContent()
        {
            var request = new NoteDto { Content = "Test", SenderId = "1", TimeCreated = DateTime.Now };

            _mockLocationRepository.Setup(x => x.GetById(1)).Returns(new StoredLocation { Id = 1, Label = "TestLoc" });

            var noteController = new NoteController(_mockNoteRepository.Object, _mockApiLogRepository.Object,
                _mockLocationRepository.Object, _mockMapper.Object);
            noteController.ControllerContext = ApiContextHelper.CreateControllerContext();
            noteController.ActionContext = ApiContextHelper.CreateActionContext();
            var result = (NegotiatedContentResult<string>)noteController.Post(1, null);
            Assert.AreEqual(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
