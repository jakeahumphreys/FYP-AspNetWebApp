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

namespace FYP_WebApp.API
{
    public class MessageController : ApiController
    {
        private readonly MessageService _messageService;
        private readonly AccountService _accountService;
        private readonly TeamService _teamService;
        private Mapper _mapper;

        public MessageController()
        {
            _accountService = new AccountService();
            _messageService = new MessageService();
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
                return Content(HttpStatusCode.BadRequest, "Invalid data submitted.");
            }
            else
            {
                var message = _mapper.Map<MessageDto, Message>(request);
                message.MessageReceived = DateTime.Now;
                var result = _messageService.SendMessage(message);

                if (result.Success)
                {
                    return Content(HttpStatusCode.Accepted, "Message received.");
                }
                else
                {
                    switch (result.ResponseError )
                    {
                        case ResponseErrors.NoValidPairing:
                            return Content(HttpStatusCode.NotFound, "No pairing found");
                        default:
                            return Content(HttpStatusCode.BadRequest, "Message not received.");

                    }
                }
            }
        }
    }

}
