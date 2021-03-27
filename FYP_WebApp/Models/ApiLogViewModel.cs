using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace FYP_WebApp.Models
{
    public class ApiLogViewModel
    {
        public ApiLog ApiLog { get; set; }
        public List<ApiLogAdditionalField> AdditionalFields { get; set; }
    }
}