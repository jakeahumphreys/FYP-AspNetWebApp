using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.DTO;
using FYP_WebApp.Hubs;
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

        public void MarkMessageAsRead(int id)
        {
            if (id != 0)
            {
                var message = _messageRepository.GetById(id);

                if (message != null)
                {
                    message.IsRead = true;

                    _messageRepository.Update(message);
                    _messageRepository.Save();
                }
            }
        }

        public ServiceResponse SendMessage(Message message)
        {
            var recipients = message.RecipientId;
            NotificationHub.Notify(message.MessageType,new List<string> { recipients }, message.Content);
            return new ServiceResponse {Success = true};
        }

        public int GetUnreadMessages(string userId)
        {
            return _messageRepository.GetAll().Count(x => x.RecipientId == userId && x.IsRead == false);
        }

        public void Dispose()
        {
            _messageRepository.Dispose();
        }
    }
}