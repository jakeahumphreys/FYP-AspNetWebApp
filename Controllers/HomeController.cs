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
        private readonly AccountService _accountService;

        public HomeController()
        {
            _messageService = new MessageService();
            _accountService = new AccountService();
        }
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                var user = _accountService.GetById(User.Identity.GetUserId());
               
                if (user != null)
                {
                    ViewBag.User = user;

                    switch (user.Roles.FirstOrDefault().RoleId)
                    {
                        case "0":
                            ViewBag.UserRole = "Administrator";
                            break;
                        case "1":
                            ViewBag.UserRole = "Manager";
                            break;
                        case "2":
                            ViewBag.UserRole = "Team Member";
                            break;
                        default:
                            ViewBag.UserRole = "New Starter / Unassigned";
                            break;
                    }

                    if (user.TeamId != null)
                    {
                        var userTeam = user.Team;
                        ViewBag.UserTeam = userTeam;
                    }

                }
             
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