using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.ServiceLayer;
using Microsoft.AspNet.Identity;

namespace FYP_WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly MessageService _messageService;

        public HomeController()
        {
            _messageService = new MessageService();
        }
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                ViewBag.unreadMessages = _messageService.GetUnreadMessages(User.Identity.GetUserId());
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}