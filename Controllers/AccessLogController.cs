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
using PagedList;

namespace FYP_WebApp.Controllers
{
    [CustomAuth(Roles = "Admin")]
    public class AccessLogController : Controller
    {
        private readonly LogService _logService;

        public AccessLogController()
        {
            _logService = new LogService();
        }
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParameter = sortOrder == "Date" ? "date_desc" : "Date";

            var accessLogs = _logService.GetAllAccessLogs();


            switch (sortOrder)
            {
                case "Date":
                    accessLogs = accessLogs.OrderBy(s => s.TimeStamp).ToList();
                    break;
                case "date_desc":
                    accessLogs = accessLogs.OrderByDescending(s => s.TimeStamp).ToList();
                    break;
                default:
                    accessLogs = accessLogs.OrderBy(s => s.Id).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(accessLogs.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_logService.GetAccessLogDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (AccessLogNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });

            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _logService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
