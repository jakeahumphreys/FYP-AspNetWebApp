using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using WebGrease.Css;

namespace FYP_WebApp.Common_Logic.Email
{
    public class MailMessageBuilder
    {
        private readonly MailMessage _mailMessage;

        public MailMessageBuilder()
        {
            _mailMessage = new MailMessage();
        }

        public MailMessageBuilder To(MailAddress address)
        {
            _mailMessage.To.Add(address);
            return this;
        }

        public MailMessageBuilder Cc(MailAddress address)
        {
            _mailMessage.CC.Add(address);
            return this;
        }

        public MailMessageBuilder Bcc(MailAddress address)
        {
            _mailMessage.Bcc.Add(address);
            return this;
        }

        public MailMessageBuilder From(MailAddress address)
        {
            _mailMessage.From = address;
            return this;
        }

        public MailMessageBuilder Subject(string subject)
        {
            _mailMessage.Subject = subject;
            return this;
        }

        public MailMessageBuilder Body(string body)
        {
            _mailMessage.Body = body;
            return this;
        }

        public MailMessageBuilder IsBodyHtml(bool isBodyHtml)
        {
            _mailMessage.IsBodyHtml = isBodyHtml;
            return this;
        }

        public MailMessage Build()
        {
            return _mailMessage;
        }
    }
}