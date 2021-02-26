using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.ServiceLayer
{
    public class MessageService
    {
        private readonly IMessageRepository _messageRepository;

        public MessageService()
        {
            _messageRepository = new MessageRepository(new ApplicationDbContext());
        }

        public MessageService(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public List<Message> GetAll()
        {
            return _messageRepository.GetAll();
        }

        public Message GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified");
            }

            var message = _messageRepository.GetById(id);

            if (message == null)
            {
                throw new MessageNotFoundException("A message with ID " + id + " was not found.");
            }

            return message;
        }

        public ServiceResponse Create(Message message)
        {
            if (message == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                _messageRepository.Insert(message);
                _messageRepository.Save();
                return new ServiceResponse {Success = true};
            }
        }

        public void Dispose()
        {
            _messageRepository.Dispose();
        }
    }
}