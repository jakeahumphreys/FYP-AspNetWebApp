using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using AutoMapper;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;

namespace FYP_WebApp.API
{
    public class MessageController : ApiController
    {
        private readonly MessageService _messageService;
        private Mapper _mapper;

        public MessageController()
        {
            _messageService = new MessageService();
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

                switch (message.MessageType)
                {
                    case MessageType.Urgent:
                        //URGENT REQUEST
                        break;
                    case MessageType.Routine:
                        break;
                }
            }
        }
    }

}
