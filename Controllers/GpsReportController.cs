using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Parser;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.AspNet.Identity;
using PagedList;

namespace FYP_WebApp.Controllers
{
    public class GpsReportController : Controller
    {
        private readonly GpsReportService _gpsReportService;
        private readonly TeamService _teamService;
        private readonly StoredLocationService _storedLocationService;

        public GpsReportController()
        {
            _gpsReportService = new GpsReportService();
            _teamService = new TeamService();
            _storedLocationService = new StoredLocationService();
        }

        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Index(string sortOrder, string currentFilter, string searchStringName, int? page)
        {
            var reportsVisible = new List<GpsReport>();

            ViewBag.CurrentSort = sortOrder;
            ViewBag.SortByNameParameter = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SortByDateParameter = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchStringName != null)
            {
                page = 1;
            }
            else
            {
                searchStringName = currentFilter;
            }

            ViewBag.CurrentFilter = searchStringName;

            if (!User.IsInRole("Admin"))
            {
                var teamIds = new List<int>();

                foreach (var team in _teamService.GetAll())
                {
                    if (team.ManagerId == User.Identity.GetUserId())
                    {
                        teamIds.Add(team.Id);
                    }
                }

                if (teamIds.Count > 0)
                {
                    var visibleReports = new List<GpsReport>();
                    foreach (var report in _gpsReportService.GetAll())
                    {
                        if (report.User.TeamId != null && teamIds.Contains((int) report.User.TeamId))
                        {
                            visibleReports.Add(report);
                        }
                    }

                    reportsVisible = visibleReports;
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = "You are not currently managing a team." });
                }
            }
            else
            {
                reportsVisible = _gpsReportService.GetAll();
            }

            if (!String.IsNullOrEmpty(searchStringName))
            {
                reportsVisible = reportsVisible.Where(s =>
                        s.User.DisplayString.IndexOf(searchStringName, StringComparison.CurrentCultureIgnoreCase) >= 0)
                    .ToList();
            }

            switch (sortOrder)
            {
                case "name_desc":
                    reportsVisible = reportsVisible.OrderByDescending(s=> s.User.Surname).ToList();
                    break;
                case "Date":
                    reportsVisible = reportsVisible.OrderBy(s => s.Time).ToList();
                    break;
                case "date_desc":
                    reportsVisible = reportsVisible.OrderByDescending(s => s.Time).ToList();
                    break;
                default:
                    reportsVisible = reportsVisible.OrderBy(s => s.Id).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(reportsVisible.ToPagedList(pageNumber, pageSize));
        }

        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Details(int id)
        {
            try
            {
                var gpsReport = _gpsReportService.Details(id);
                var mapsApiKey = ConfigHelper.GetLatestConfigRecord().MapsApiKey;
                ViewBag.mapUrl = $"https://www.google.com/maps/embed/v1/place?key={mapsApiKey}&q={gpsReport.Latitude},{gpsReport.Longitude}";
                return View(gpsReport);
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (GpsReportNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }


        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewBag.LocationList = new SelectList(_storedLocationService.Index().Where(l => l.IsInactive == false), "Id", "Label");

                return View(_gpsReportService.Details(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (StoredLocationNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Edit(GpsReport gpsReport)
        {
            ViewBag.LocationList = new SelectList(_gpsReportService.GetAll(), "Id", "Label");
            if (ModelState.IsValid)
            {
                ServiceResponse response = _gpsReportService.Edit(gpsReport);

                if (response.Success == true)
                {
                    return RedirectToAction("Details", "GpsReport", new {id = gpsReport.Id});
                }
                else
                {
                    return View(gpsReport);
                }
            }
            return View(gpsReport);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _gpsReportService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
