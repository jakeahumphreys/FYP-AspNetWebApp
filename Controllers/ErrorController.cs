using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;

namespace FYP_WebApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult Index(string message)
        {
            ViewBag.message = message;
            return View();
        }

        public ActionResult Error(Errors error, string message)
        {
            return View(new ErrorViewModel {Error = error, Message = message});
        }
    }
}