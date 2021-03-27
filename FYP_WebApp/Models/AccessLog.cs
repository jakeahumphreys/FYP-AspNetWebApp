using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class AccessLog
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string AttemptedUser { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}