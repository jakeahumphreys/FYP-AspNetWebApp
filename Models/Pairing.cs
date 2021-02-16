using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class Pairing
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string BuddyUserId { get; set; }
        public ApplicationUser BuddyUser { get; set; }
    }
}