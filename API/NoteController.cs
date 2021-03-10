using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json;

namespace FYP_WebApp.API
{
    public class NoteController : ApiController
    {
        private readonly NoteService _noteService;
        private readonly LogService _logService;
        private readonly StoredLocationService _storedLocationService;
        private readonly Mapper _mapper;

        public NoteController()
        {
            _noteService = new NoteService();
            _logService = new LogService();
            _storedLocationService = new StoredLocationService();
            var config = AutomapperConfig.instance().Configure();
            _mapper = new Mapper(config);
        }

        [HttpGet]
        public IEnumerable<NoteDto> Get()
        {
            var response =  _mapper.Map<IList<Note>, List<NoteDto>>(_noteService.GetAll());

            _logService.CreateApiLog(new ApiLog
            {
                LogLevel = LogLevel.Info,
                Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                Action = this.ActionContext.ActionDescriptor.ActionName,
                TimeStamp = DateTime.Now,
                RequestString = "None",
                ResponseString = JsonConvert.SerializeObject(response, Formatting.Indented),
                StatusCode = HttpStatusCode.OK.ToString()
            });

            return response;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                var response = Json(_mapper.Map<Note, NoteDto>(_noteService.GetDetails(id)));

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Info,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = id.ToString(),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.OK.ToString()
                });

                return response;
            }
            catch (ArgumentException ex)
            {
                var response = Content(HttpStatusCode.BadRequest, ex.Message);

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = id.ToString(),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
            }
            catch (NoteNotFoundException ex)
            {
                var response = Content(HttpStatusCode.NotFound, ex.Message);

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = id.ToString(),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.NotFound.ToString()
                });

                return response;
            }
        }

        [HttpPost]
        public IHttpActionResult Post(int storedLocationId, [FromBody] NoteDto requestBody)
        {
            if (storedLocationId == 0)
            {
                var response = Content(HttpStatusCode.BadRequest, "A Location ID is required to add a note.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject(new { storedLocationId = storedLocationId, requestBody = requestBody}, Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
;           }

            if (requestBody == null)
            {
                var response = Content(HttpStatusCode.BadRequest, "No body was sent with the request.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject(new { storedLocationId = storedLocationId, requestBody = "NULL" }, Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
            }

            if (_storedLocationService.GetDetails(storedLocationId) == null)
            {
                var response = Content(HttpStatusCode.BadRequest, "A location with the specified ID does not exist.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject(new { storedLocationId = storedLocationId, requestBody = requestBody }, Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
            }

            var note = _mapper.Map<NoteDto, Note>(requestBody);
            note.StoredLocationId = storedLocationId;
            var result = _noteService.Create(note);

            if (result.Success)
            {
                var response = Content(HttpStatusCode.OK, "Note added successfully.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Info,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject(new { storedLocationId = storedLocationId, requestBody = requestBody }, Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.OK.ToString()
                });

                return response;
            }
            else
            {
                switch (result.ResponseError)
                {
                    case ResponseErrors.NullParameter:
                        var response = Content(HttpStatusCode.BadRequest, "The request body provided was invalid.");

                        _logService.CreateApiLog(new ApiLog
                        {
                            LogLevel = LogLevel.Error,
                            Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                            Action = this.ActionContext.ActionDescriptor.ActionName,
                            TimeStamp = DateTime.Now,
                            RequestString = JsonConvert.SerializeObject(requestBody, Formatting.Indented),
                            ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                            StatusCode = HttpStatusCode.BadRequest.ToString()
                        });

                        return response;
                    default:
                        return BadRequest();
                }

            }
        }


    }
}
