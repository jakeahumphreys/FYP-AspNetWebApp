using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;

namespace FYP_WebApp.DTO
{
    public class MapDto
    {
        public string Name { get; set; }
        public Decimal Latitude { get; set; }
        public Decimal Longitude { get; set; }
        public string Status { get; set; }
    }
}