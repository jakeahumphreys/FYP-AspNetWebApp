using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;

namespace FYP_WebApp.DTO
{
    [DataContract(Name = "Message")]
    public class MessageDto
    {
        [DataMember(Name = "Message_ID")]
        public int Id { get; set; }
        [DataMember(Name = "Message_Type")]
        public MessageType MessageType { get; set; }
        [DataMember(Name = "Message_Sender_ID")]
        public string SenderId { get; set; }
        [DataMember(Name = "Message_Content")]
        public string Content { get; set; }
    }
}