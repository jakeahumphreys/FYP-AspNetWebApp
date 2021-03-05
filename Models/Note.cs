using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int StoredLocationId { get; set; }
        public StoredLocation StoredLocation { get; set; }
        public bool IsInactive { get; set; }
    }
}