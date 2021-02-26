using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Common_Logic
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public object ServiceObject { get; set; }
        public ResponseErrors ResponseError { get; set; }
    }
}