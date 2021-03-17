using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Common_Logic.Email;
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
        private readonly IPairingRepository _pairingRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly ApplicationDbContext _applicationDbContext;

        public MessageService()
        {
            _messageRepository = new MessageRepository(new ApplicationDbContext());
            _pairingRepository = new PairingRepository(new ApplicationDbContext());
            _teamRepository = new TeamRepository(new ApplicationDbContext());
            _applicationDbContext = new ApplicationDbContext();
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
            List<String> recipients = new List<string>();

            if (message.MessageType == MessageType.CheckIn)
            {
                //item.Start <= DateTime.Now && item.End >= DateTime.Now
                var pairing = _pairingRepository
                    .GetAll()
                    .FirstOrDefault(x => x.UserId == message.SenderId && x.Start <= DateTime.Now && x.End >= DateTime.Now);
                if (pairing == null)
                {
                    return new ServiceResponse {Success = false, ResponseError = ResponseErrors.NoValidPairing};
                }

                recipients.Add(pairing.BuddyUserId);
            }

            if (message.MessageType == MessageType.Urgent)
            {
                var user = _applicationDbContext.Users.Find(message.SenderId);

                if (user.TeamId == null)
                {
                    recipients = GetAllManagers();
                    message.Content += " [FAO ALL MANAGERS -- NO TEAM]";
                }
                else
                {
                    var team = _teamRepository.GetById((int)user.TeamId);

                    if (team.ManagerId == null)
                    {
                        recipients = GetAllManagers();
                        message.Content += " [FAO ALL MANAGERS -- NO TEAM]";
                    }
                    else
                    {
                        recipients.Add(team.ManagerId);
                    }
                }
            }

            if (message.MessageType == MessageType.Routine)
            {
                recipients.Add(message.RecipientId);
            }

            NotificationHub.Notify(message.MessageType,recipients, message.Content);

            foreach (var recipient in recipients)
            {
                message.RecipientId = recipient;
                Create(message);

                if (message.MessageType == MessageType.Urgent)
                {
                    var user = _applicationDbContext.Users.Find(recipient);
                    var sender = _applicationDbContext.Users.Find(message.SenderId);
                    if (user.NotifyEmail != null)
                    {
                        SendUrgentEmail(ConfigHelper.GetLatestConfigRecord(), 
                            user.NotifyEmail,
                            "Urgent Assistance Request",
                            $"{sender.DisplayString} has requested urgent assistance at {DateTime.Now}.",
                            false
                        );
                    }
                }
            }

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

        public List<string> GetAllManagers()
        {
            List<string> allManagers = new List<string>();

            foreach (var team in _teamRepository.GetAll())
            {
                if (team.ManagerId != null)
                {
                    allManagers.Add(team.ManagerId);
                }
            }

            return allManagers;
        }

        public void SendUrgentEmail(ConfigurationRecord latestConfig, string toAddress, string subject, string body, bool isHtml)
        {
            if (latestConfig.SmtpSendUrgentEmails)
            {
                var smtpClientBuilder = new SmtpClientBuilder();
                var smtpClient = smtpClientBuilder
                    .Host(latestConfig.SmtpUrl)
                    .Port(latestConfig.SmtpPort)
                    .Credentials(new NetworkCredential(latestConfig.SmtpSenderUsername, latestConfig.SmtpSenderPassword))
                    .EnableSsl(latestConfig.SmtpShouldUseSsl)
                    .Build();

                var mailMessageBuilder = new MailMessageBuilder();
                var mailMessage = mailMessageBuilder
                    .To(new MailAddress(toAddress))
                    .From(new MailAddress(latestConfig.SmtpEmailFrom))
                    .Subject(subject)
                    .Body(body)
                    .IsBodyHtml(isHtml)
                    .Build();

                smtpClient.Send(mailMessage);
            }
         
        }
    }
}