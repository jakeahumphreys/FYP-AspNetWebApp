using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class Team
    {
        public int Id { get; set; }
        [Display(Name = "Team Name")]
        public string Name { get; set; }
        public ICollection<ApplicationUser> Members { get; set; }
        public bool IsInactive { get; set; }
    }
}