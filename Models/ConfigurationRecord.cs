using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using System.Web.Http.Routing.Constraints;

namespace FYP_WebApp.Models
{
    public class ConfigurationRecord
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }
        [Required]
        [Display(Name = "Store Location Under X Metres")]
        public double StoreLocationAccuracy { get; set; }
        [Display(Name = "System wide Message of the Day")]
        public string MessageOfTheDayText { get; set; }
    }
}