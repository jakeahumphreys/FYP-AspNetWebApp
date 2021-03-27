using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Owin.Security.Facebook;
using Newtonsoft.Json;
using PagedList;

namespace FYP_WebApp.Controllers
{
    [CustomAuth(Roles = "Admin")]
    public class ApiLogController : Controller
    {

        private readonly LogService _logService;

        public ApiLogController()
        {
            _logService = new LogService();
        }

        public ApiLogController(IApiLogRepository apiLogRepository)
        {
            _logService = new LogService(apiLogRepository);
        }

        // GET: ApiLog
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.DateSortParameter = sortOrder == "Date" ? "date_desc" : "Date";

            var apiLogs = _logService.GetAllApiLogs();


            switch (sortOrder)
            {
                case "Date":
                    apiLogs = apiLogs.OrderBy(s => s.TimeStamp).ToList();
                    break;
                case "date_desc":
                    apiLogs = apiLogs.OrderByDescending(s => s.TimeStamp).ToList();
                    break;
                default:
                    apiLogs = apiLogs.OrderBy(s => s.Id).ToList();
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            ViewBag.TotalApiCalls = apiLogs.Count;
            ViewBag.TotalApiErrors = apiLogs.Count(x => x.LogLevel == LogLevel.Error);
            ViewBag.TotalApiInfos = apiLogs.Count(x => x.LogLevel == LogLevel.Info);

            return View(apiLogs.ToPagedList(pageNumber, pageSize));
        }

        // GET: ApiLog/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var apiLog = _logService.GetApiLogDetails(id);
                var additionalFields = _logService.ConvertFieldStringToList(apiLog.AdditionalFields);

                var apiLogViewModel = new ApiLogViewModel {ApiLog = apiLog, AdditionalFields = additionalFields};

                return View(apiLogViewModel);
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new {@Error = Errors.InvalidParameter, @Message = ex.Message});
            }
            catch (ApiLogNotFoundException ex)
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
