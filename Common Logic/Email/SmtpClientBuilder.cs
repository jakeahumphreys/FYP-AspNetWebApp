using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.Common_Logic.Email
{
    public class SmtpClientBuilder
    {
        private readonly SmtpClient _smtpClient;

        public SmtpClientBuilder()
        {
            _smtpClient = new SmtpClient();
        }

        public SmtpClientBuilder Host(string hostAddress)
        {
            _smtpClient.Host = hostAddress;
            return this;
        }

        public SmtpClientBuilder Port(int port)
        {
            _smtpClient.Port = port;
            return this;
        }

        public SmtpClientBuilder Credentials(NetworkCredential credentials)
        {
            _smtpClient.Credentials = credentials;
            return this;
        }

        public SmtpClientBuilder EnableSsl(bool isEnabled)
        {
            _smtpClient.EnableSsl = isEnabled;
            return this;
        }

        public SmtpClient Build()
        {
            return _smtpClient;
        }
    }
}