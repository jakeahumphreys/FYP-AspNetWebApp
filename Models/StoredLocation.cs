using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class StoredLocation
    {
        public int Id { get; set; }
        public string Label { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal Latitude { get; set; }
        [DisplayFormat(DataFormatString = "{0:0.000000}", ApplyFormatInEditMode = true)]
        public decimal Longitude { get; set; }
        public List<Note> Notes { get; set; }
        public bool IsInactive { get; set; }
    }
}