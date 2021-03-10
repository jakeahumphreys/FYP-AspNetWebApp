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
        [Display(Name = "Log Level")]
        public LogLevel LogLevel { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public DateTime TimeStamp { get; set; }
        [Display(Name = "Request")]
        public string RequestString { get; set; }
        [Display(Name = "Response")]
        public string ResponseString { get; set; }
        [Display(Name = "Status Code")]
        public string StatusCode { get; set; }
    }
}