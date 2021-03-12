using FYP_WebApp.Common_Logic;
using FYP_WebApp.ServiceLayer;
using Microsoft.AspNet.Identity;
using System;
using System.Linq;
using System.Web.Mvc;

namespace FYP_WebApp.Controllers
{
    [CustomAuth(Roles = "Admin, Manager, Member")]
    public class MessageController : Controller
    {
        private readonly MessageService _messageService;

        public MessageController()
        {
            _messageService = new MessageService();
        }

        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            return View(_messageService.GetAll().Where(x=>x.RecipientId == userId));
        }

        public ActionResult Details(int id)
        {
            try
            {
                _messageService.MarkMessageAsRead(id);
                return View(_messageService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (MessageNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _messageService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}