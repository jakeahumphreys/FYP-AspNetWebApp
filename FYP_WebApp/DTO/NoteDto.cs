using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DTO
{
    [DataContract(Name = "Note")]
    public class NoteDto
    {
        [DataMember(Name = "Note_Content")]
        public string Content { get; set; }
        [DataMember(Name = "Note_Sender_ID")]
        public string SenderId { get; set; }
        [DataMember(Name = "Note_Time_Created")]
        public DateTime TimeCreated { get; set; }
    }
}