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
        private readonly Library _library;

        public PairingController()
        {
            _pairingService = new PairingService();
            _accountService = new AccountService();
            _teamService = new TeamService();
            _library = new Library();
        }

        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Index()
        {
            return View(_pairingService.GetAll());
        }

        [CustomAuth(Roles = "Admin, Manager")]
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

        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Create()
        {
            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()).ToList(), "Id", "DisplayString");
            var pairingViewModel = new PairingViewModel {UserList = userList};
            return View(pairingViewModel);
        }

        [CustomAuth(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartDate, StartTime, EndDate, EndTime, Pairing, UserList, ConflictingPairings")] PairingViewModel pairingViewModel)
        {
            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()), "Id", "DisplayString");
            pairingViewModel.UserList = userList;

            if (ModelState.IsValid)
            {
                var conflictingPairings = _pairingService.CheckConflictingPairings(pairingViewModel.Pairing);

                if (conflictingPairings != null && conflictingPairings.Count > 0)
                {
                    pairingViewModel.ConflictingPairings = conflictingPairings;
                    return View(pairingViewModel);
                }

                pairingViewModel.Pairing.Start =
                    _library.ConstructDateTime(pairingViewModel.StartDate, pairingViewModel.StartTime);
                pairingViewModel.Pairing.End =
                    _library.ConstructDateTime(pairingViewModel.EndDate, pairingViewModel.EndTime);

                var result = _pairingService.Create(pairingViewModel.Pairing);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(pairingViewModel);
                }
            }

            return View(pairingViewModel);
        }

        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Edit(int id)
        {
            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()), "Id", "DisplayString");

            try
            {
                return View(_pairingService.ConstructViewModel(_pairingService.GetDetails(id), userList));
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

        [CustomAuth(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id, UserId, BuddyUserId, StartDate, StartTime, EndDate, EndTime, Pairing, UserList, ConflictingPairings")] PairingViewModel pairingViewModel)
        {
            var userList = new SelectList(_teamService.GetSubordinates(_accountService.GetAll(), User.Identity.GetUserId()).ToList(), "Id", "DisplayString");
            pairingViewModel.UserList = userList;

            if (ModelState.IsValid)
            {
                pairingViewModel.Pairing.Start =
                    _library.ConstructDateTime(pairingViewModel.StartDate, pairingViewModel.StartTime);
                pairingViewModel.Pairing.End =
                    _library.ConstructDateTime(pairingViewModel.EndDate, pairingViewModel.EndTime);

                var conflictingPairings = _pairingService.CheckConflictingPairings(pairingViewModel.Pairing);

                if (conflictingPairings != null && conflictingPairings.Count > 0)
                {
                    pairingViewModel.ConflictingPairings = conflictingPairings;
                    return View(pairingViewModel);
                }

                var result = _pairingService.Edit(pairingViewModel.Pairing);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(pairingViewModel);
                }
            }

            return View(pairingViewModel);
        }

        [CustomAuth(Roles = "Admin, Manager")]
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

        [CustomAuth(Roles = "Admin, Manager")]
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