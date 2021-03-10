using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;

namespace FYP_WebApp.Models
{
    public class ApiLog
    {
        public int Id { get; set; }
        public LogLevel LogLevel { get; set; }
        public DateTime TimeStamp { get; set; }
        public string RequestString { get; set; }
        public string ResponseString { get; set; }
        public string StatusCode { get; set; }
    }
}