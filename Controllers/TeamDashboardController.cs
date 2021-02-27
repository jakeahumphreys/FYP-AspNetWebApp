using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.Controllers
{
    public class TeamDashboardController : Controller
    {

        private AccountService _accountService;
        private TeamService _teamService;

        public TeamDashboardController()
        {
            _accountService = new AccountService();
            _teamService = new TeamService();
        }

        public ActionResult Index()
        {
            var teams = _teamService.GetUserTeams(User.Identity.GetUserId());

            if (teams != null && teams.Count > 0)
            {
                if (teams.Count > 1)
                {
                    return RedirectToAction("TeamSelect", "TeamDashboard");
                }
                else
                {
                    return RedirectToAction("Manage", "TeamDashboard", new {teamId = teams[0].Id});
                }
            }
            else
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = "You do not manage a team." });
            }
        }

        public ActionResult TeamSelect()
        {
            var teamSelectVm = new TeamSelectViewModel {teams = _teamService.GetUserTeams(User.Identity.GetUserId())};
            return View(teamSelectVm);
        }

        public ActionResult Manage(int teamId)
        {
            var team = _teamService.GetDetails(teamId);
            var userId = User.Identity.GetUserId();
            var members = _accountService.GetAll().Where(x => x.TeamId == teamId).Where(x=> x.Id != userId).ToList();
            var teamDashboardVm = new TeamDashboardViewModel {Team = team, Members = members};

            return View(teamDashboardVm);
        }
    }
}