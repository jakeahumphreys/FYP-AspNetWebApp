using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Models
{
    public class TeamDashboardViewModel
    {
        public Team Team { get; set; }
        public List<ApplicationUser> Members { get; set; }
    }
}