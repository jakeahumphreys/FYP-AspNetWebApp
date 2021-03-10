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

        public GpsReportController()
        {
            _gpsReportService = new GpsReportService();
            _teamService = new TeamService();
        }

        [Authorize(Roles="Admin, Manager")]
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
                var team = _teamService.GetAll().FirstOrDefault(x => x.ManagerId == User.Identity.GetUserId());
                if (team != null)
                {
                    reportsVisible = _gpsReportService.GetAll().Where(x => x.User.TeamId == team.Id).ToList();
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

        // GET: GpsReport/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var gpsReport = _gpsReportService.Details(id);
                ViewBag.mapUrl = $"https://www.google.com/maps/embed/v1/place?key=AIzaSyCHpCaID-NBJ4ww4_PZewLLttqi2iKAIQ8&q={gpsReport.Latitude},{gpsReport.Longitude}";
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
