using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.AspNet.Identity;

namespace FYP_WebApp.Controllers
{
    public class PairingController : Controller
    {

        private readonly PairingService _pairingService;
        private readonly AccountService _accountService;
        private readonly TeamService _teamService;

        public PairingController()
        {
            _pairingService = new PairingService();
            _accountService = new AccountService();
            _teamService = new TeamService();
        }

        public ActionResult Index()
        {
            return View(_pairingService.GetAll());
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_pairingService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new {@Error = Errors.InvalidParameter, @Message = ex.Message});
            }
            catch (PairingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()).ToList(), "Id", "DisplayString");
            ViewBag.userList = userList;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id, Start, End, UserId, BuddyUserId")] Pairing pairing)
        {
            if (ModelState.IsValid)
            {
                var result = _pairingService.Create(pairing);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(pairing);
                }
            }

            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()), "Id", "DisplayString");
            ViewBag.userList = userList;
            return View(pairing);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()), "Id", "DisplayString");
                ViewBag.userList = userList;
                return View(_pairingService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (PairingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, Start, End, UserId, BuddyUserId")] Pairing pairing)
        {
            if (ModelState.IsValid)
            {
                var result = _pairingService.Edit(pairing);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(pairing);
                }
            }

            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()).ToList(), "Id", "DisplayString");
            ViewBag.userList = userList;
            return View(pairing);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                return View(_pairingService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (PairingNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pairing pairing = _pairingService.GetDetails(id);

            var result = _pairingService.Delete(pairing);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.SystemError, @Message = "Unable to delete entity." });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _pairingService.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}