using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Common_Logic
{
    public class StoredLocationNotFoundException : Exception
    {
        public StoredLocationNotFoundException(string message) : base(message) { }
    }
}