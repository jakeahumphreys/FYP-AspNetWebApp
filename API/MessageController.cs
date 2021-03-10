using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Hubs;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json;

namespace FYP_WebApp.API
{
    public class MessageController : ApiController
    {
        private readonly MessageService _messageService;
        private readonly AccountService _accountService;
        private readonly LogService _logService;
        private readonly TeamService _teamService;
        private Mapper _mapper;

        public MessageController()
        {
            _accountService = new AccountService();
            _messageService = new MessageService();
            _logService = new LogService();
            _teamService = new TeamService();
            var config = AutomapperConfig.instance().Configure();
            _mapper = new Mapper(config);
        }

        [System.Web.Http.HttpGet]
        public IEnumerable<MessageDto> Get()
        {
            return _mapper.Map<IList<Message>, List<MessageDto>>(_messageService.GetAll());
        }


        [System.Web.Http.HttpPost]
        public IHttpActionResult Post([FromBody] MessageDto request)
        {
            if (request == null)
            {
                var response = Content(HttpStatusCode.BadRequest, "Invalid data submitted.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = "NULL",
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
            }
            else
            {
                var message = _mapper.Map<MessageDto, Message>(request);
                message.MessageReceived = DateTime.Now;
                var result = _messageService.SendMessage(message);

                if (result.Success)
                {
                    var response = Content(HttpStatusCode.Accepted, "Message received.");

                    _logService.CreateApiLog(new ApiLog
                    {
                        LogLevel = LogLevel.Info,
                        Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                        Action = this.ActionContext.ActionDescriptor.ActionName,
                        TimeStamp = DateTime.Now,
                        RequestString = JsonConvert.SerializeObject(request, Formatting.Indented),
                        ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                        StatusCode = HttpStatusCode.Accepted.ToString()
                    });

                    return response;
                }
                else
                {
                    switch (result.ResponseError )
                    {
                        case ResponseErrors.NoValidPairing:
                            var response = Content(HttpStatusCode.NotFound, "No pairing found");

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
                        default:
                            return Content(HttpStatusCode.BadRequest, "Message not received.");
                    }
                }
            }
        }
    }

}
