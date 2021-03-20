using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Core.Mapping;
using System.Linq;
using System.Web;
using System.Web.Http.Routing.Constraints;
using Foolproof;

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

        //Email and SMTP
        [Display(Name = "Send Urgent Emails")]
        public bool SmtpSendUrgentEmails { get; set; }
        [RequiredIfTrue("SmtpSendUrgentEmails")]
        [Display(Name = "SMTP Server URL")]
        public string SmtpUrl { get; set; }
        [RequiredIfTrue("SmtpSendUrgentEmails")]
        [Display(Name = "SMTP Port")]
        public int? SmtpPort { get; set; }
        [RequiredIfTrue("SmtpSendUrgentEmails")]
        [Display(Name = "SMTP Email From")]
        [DataType(DataType.EmailAddress)]
        public string SmtpEmailFrom { get; set; }
        [RequiredIfTrue("SmtpSendUrgentEmails")]
        [Display(Name = "SMTP Sender Username")]
        public string SmtpSenderUsername { get; set; }
        [RequiredIfTrue("SmtpSendUrgentEmails")]
        [DataType(DataType.Password)]
        [Display(Name = "SMTP Sender Password")]
        public string SmtpSenderPassword { get; set; }
        [Display(Name = "Use SSL")]
        public bool SmtpShouldUseSsl { get; set; }

        //Google Maps
        [Display(Name = "Google Maps API Key")]
        public string MapsApiKey { get; set; }

    }
}