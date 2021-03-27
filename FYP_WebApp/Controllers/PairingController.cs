using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
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

        public PairingController(IPairingRepository pairingRepository, ApplicationDbContext context, ITeamRepository teamRepository, Library library)
        {
            _pairingService = new PairingService(pairingRepository);
            _accountService = new AccountService(context);
            _teamService = new TeamService(teamRepository);
            _library = library;
        }

        [CustomAuth(Roles = "Admin, Manager")]
        public ActionResult Index()
        {
            var managerId = User.Identity.GetUserId();

            //List<Pairing> visiblePairings = _pairingService.GetAll().Where(x => x.User.TeamId != null && x.User.Team.ManagerId == managerId && x.BuddyUser.TeamId != null && x.BuddyUser.Team.ManagerId == managerId).ToList();

            var visiblePairings = new List<Pairing>();

            var teamMembers = _teamService.GetAllSubordinates(_accountService.GetAll(), managerId);

            foreach (var member in teamMembers)
            {
                foreach (var pairing in _pairingService.GetAll())
                {
                    if (pairing.UserId == member.Id || pairing.BuddyUserId == member.Id)
                    {
                        if (!visiblePairings.Contains(pairing))
                        {
                            visiblePairings.Add(pairing);
                        }
                    }
                }
            }

            if (teamMembers.Count == 0)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = "You currently have no team members to pair." });
            }

            return View(visiblePairings);

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
            var userList = new SelectList(_teamService.GetAllSubordinates(_accountService.GetAll().Where(x=> x.IsInactive == false).ToList(), User.Identity.GetUserId()).ToList(), "Id", "DisplayString");
            var pairingViewModel = new PairingViewModel {UserList = userList};
            return View(pairingViewModel);
        }

        [CustomAuth(Roles = "Admin, Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StartDate, StartTime, EndDate, EndTime, Pairing, UserList, ConflictingPairings")] PairingViewModel pairingViewModel)
        {
            var userList = new SelectList(_teamService.GetAllSubordinates(_accountService.GetAll().Where(x => x.IsInactive == false).ToList(), User.Identity.GetUserId()), "Id", "DisplayString");
            pairingViewModel.UserList = userList;

            if (ModelState.IsValid)
            {
                var conflictingPairings = _pairingService.CheckConflictingPairings(pairingViewModel.Pairing);

                if (conflictingPairings != null && conflictingPairings.Count > 0)
                {
                    pairingViewModel.ConflictingPairings = conflictingPairings;
                    return View(pairingViewModel);
                }

                if (pairingViewModel.Pairing == null)
                {
                    return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = "No Pairing." });
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
            var userList = new SelectList(_teamService.GetAllSubordinates(_accountService.GetAll().Where(x => x.IsInactive == false).ToList(), User.Identity.GetUserId()), "Id", "DisplayString");

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
            var userList = new SelectList(_teamService.GetAllSubordinates(_accountService.GetAll().Where(x => x.IsInactive == false).ToList(), User.Identity.GetUserId()).ToList(), "Id", "DisplayString");
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