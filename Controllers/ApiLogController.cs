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

namespace FYP_WebApp.Controllers
{
    public class ApiLogController : Controller
    {

        private readonly LogService _logService;

        public ApiLogController()
        {
            _logService = new LogService();
        }
        // GET: ApiLog
        public ActionResult Index()
        {
            return View(_logService.GetAllApiLogs());
        }

        // GET: ApiLog/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                return View(_logService.GetApiLogDetails(id));
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
