using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Newtonsoft.Json;

namespace FYP_WebApp.API
{
    public class StatusController : ApiController
    {
        private readonly AccountService _accountService;
        private readonly LogService _logService;

        public StatusController()
        {
            _accountService = new AccountService();
            _logService = new LogService();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] StatusDto request)
        {
            if (request == null)
            {
                var response = Content(HttpStatusCode.BadRequest, "Invalid data submitted");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject("NULL", Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
            }
            else
            {
                var result = _accountService.SetUserStatus(request.UserId, request.status);

                if (result.Success)
                {
                    var response = Content(HttpStatusCode.OK, "User status updated to " + request.status);

                    _logService.CreateApiLog(new ApiLog
                    {
                        LogLevel = LogLevel.Info,
                        Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                        Action = this.ActionContext.ActionDescriptor.ActionName,
                        TimeStamp = DateTime.Now,
                        RequestString = JsonConvert.SerializeObject(request, Formatting.Indented),
                        ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                        StatusCode = HttpStatusCode.OK.ToString()
                    });

                    return response;
                }
                else
                {
                    switch (result.ResponseError)
                    {
                        case ResponseErrors.EntityNotFound:
                            var response = Content(HttpStatusCode.NotFound, "A user with the specified ID " + request.UserId + " was not found.");

                            _logService.CreateApiLog(new ApiLog
                            {
                                LogLevel = LogLevel.Error,
                                Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                                Action = this.ActionContext.ActionDescriptor.ActionName,
                                TimeStamp = DateTime.Now,
                                RequestString = JsonConvert.SerializeObject(request, Formatting.Indented),
                                ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                                StatusCode = HttpStatusCode.NotFound.ToString()
                            });

                            return response;
                        case ResponseErrors.NullParameter:
                            response = Content(HttpStatusCode.BadRequest, "One of the specified parameters was null or invalid.");

                            _logService.CreateApiLog(new ApiLog
                            {
                                LogLevel = LogLevel.Error,
                                Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                                Action = this.ActionContext.ActionDescriptor.ActionName,
                                TimeStamp = DateTime.Now,
                                RequestString = JsonConvert.SerializeObject(request, Formatting.Indented),
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
}
