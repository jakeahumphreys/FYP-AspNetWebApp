using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.Controllers
{
    [CustomAuth(Roles = "Manager")]
    public class TeamDashboardController : Controller
    {

        private readonly AccountService _accountService;
        private readonly TeamService _teamService;
        private readonly GpsReportService _gpsReportService;
        private readonly PairingService _pairingService;
        private readonly ConfigurationRecordService _configRecordService;

        private int CurrentTeamId { get; set; }

        public TeamDashboardController()
        {
            _accountService = new AccountService();
            _teamService = new TeamService();
            _gpsReportService = new GpsReportService();
            _pairingService = new PairingService();
            _configRecordService = new ConfigurationRecordService();
        }

        public TeamDashboardController(ApplicationDbContext context, ITeamRepository teamRepository, IGpsReportRepository gpsReportRepository, IPairingRepository pairingRepository, IConfigurationRecordRepository configRecordRepository)
        {
            _accountService = new AccountService(context);
            _teamService = new TeamService(teamRepository);
            _gpsReportService = new GpsReportService(gpsReportRepository);
            _pairingService = new PairingService(pairingRepository);
            _configRecordService = new ConfigurationRecordService(configRecordRepository);
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

        public JsonResult GetGpsLocations(int teamId)
        {
            var mapObjects = new List<MapDto>();
            var userId = User.Identity.GetUserId();
            var gpsReports = _gpsReportService.GetGpsReports(_teamService.GetTeamSubordinates(_accountService.GetAll().Where(x=> x.Id != userId && x.Status != Status.NotOnShift).ToList(), userId, teamId));

            foreach (var report in gpsReports)
            {
                var user = _accountService.GetById(report.UserId);
                mapObjects.Add(new MapDto
                {
                    Name = user.DisplayString,
                    Latitude = report.Latitude,
                    Longitude = report.Longitude,
                    Status = Enum.GetName(typeof(Status), user.Status)
                });
            }
            return Json(mapObjects, JsonRequestBehavior.AllowGet);
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
            var members = _accountService.GetAll().Where(x => x.TeamId == teamId && x.IsInactive == false).Where(x=> x.Id != userId).ToList();
            var onDutyMembers = members.Count(x=> x.Status != Status.NotOnShift);
            var unlinkedReports = _gpsReportService.GetAll().Where(x => x.User.TeamId == teamId && x.LocationId == null).ToList();
            var unpairedMembers = _pairingService.GetDailyUnpairedUsers(members);
            var teamDashboardVm = new TeamDashboardViewModel {Team = team, Members = members, OnDutyMembers = onDutyMembers, UnlinkedReports = unlinkedReports, UnpairedTeamMembers = unpairedMembers, MapsApiKey = _configRecordService.GetLatestConfigurationRecord().MapsApiKey};


            return View(teamDashboardVm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _teamService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}