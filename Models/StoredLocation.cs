using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class StoredLocation
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public ICollection<Note> Notes { get; set; }
        public bool IsInactive { get; set; }
    }
}