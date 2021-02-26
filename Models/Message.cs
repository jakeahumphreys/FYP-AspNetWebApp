using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;
using Microsoft.Owin.Security;

namespace FYP_WebApp.Models
{
    public class Message
    {
        public int Id { get; set; }
        [EnumDataType(typeof(MessageType))]
        public MessageType MessageType { get; set; }
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }
        public string RecipientId { get; set; }
        public ApplicationUser Recipient { get; set; }
        public string Content { get; set; }
        public bool IsRead { get; set; }
    }
}