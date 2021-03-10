using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.AspNet.Identity;

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
        public ActionResult Index()
        {
            if (!User.IsInRole("Admin"))
            {
                var team = _teamService.GetAll().FirstOrDefault(x => x.ManagerId == User.Identity.GetUserId());
                if (team != null)
                {
                    return View(_gpsReportService.GetAll().Where(x => x.User.TeamId == team.Id));
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = "You are not currently managing a team." });
                }
            }
            else
            {
                return View(_gpsReportService.GetAll());
            }
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
