using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace FYP_WebApp.Models
{
    public class Pairing
    {
        public int Id { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Start { get; set; }
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd H:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime End { get; set; }
        [NotEqualTo("BuddyUserId", ErrorMessage = "Cannot be the same as Buddy User.")]
        [Display(Name = "Team Member")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        [NotEqualTo("UserId", ErrorMessage = "Cannot be the same as User.")]
        [Display(Name = "Assigned Buddy")]
        public string BuddyUserId { get; set; }
        public ApplicationUser BuddyUser { get; set; }

        public string DisplayString =>
            "[Pairing " + Id + "]: " + Start.Date + " - " + End.Date + " (" + User.Surname + ", " +
            BuddyUser.Surname + ")";
    }
}