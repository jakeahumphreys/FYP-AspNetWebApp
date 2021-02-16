using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class ConfigurationRecord
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public bool IsInactive { get; set; }
    }
}