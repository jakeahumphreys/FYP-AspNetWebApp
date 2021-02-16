using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class GpsReport
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}