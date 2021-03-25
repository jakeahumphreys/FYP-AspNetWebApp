using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.Controllers
{
    [CustomAuth(Roles= "Admin")]
    public class TeamController : Controller
    {
        private readonly TeamService _teamService;
        private readonly AccountService _accountService;

        public TeamController()
        {
            _teamService = new TeamService();
            _accountService = new AccountService();
        }

        public ActionResult Index()
        {
            return View(_teamService.GetAll());
        }

        public ActionResult Details(int id)
        {
            try
            {
                var team = _teamService.GetDetails(id);
                team.TeamMembers = GetTeamMembers(team.Id, team.ManagerId);
                return View(team);
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (TeamNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            var managerList = new SelectList(_accountService.GetAllManagers(), "Id", "DisplayString");
            ViewBag.managerList = managerList;
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,ManagerId,IsInactive")] Team team)
        {
            var managerList = new SelectList(_accountService.GetAllManagers(), "Id", "DisplayString");
            ViewBag.managerList = managerList;

            if (ModelState.IsValid)
            {
                var result = _teamService.Create(team);

                if (result.Success)
                {
                    return Redirect("Index");
                }
                else
                {
                    return View(team);
                }
            }

            return View(team);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                var managerList = new SelectList(_accountService.GetAllManagers(), "Id", "DisplayString");
                ViewBag.managerList = managerList;
                return View(_teamService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (TeamNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name, ManagerId, IsInactive")] Team team)
        {
            if (ModelState.IsValid)
            {
                var result = _teamService.Edit(team);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(team);
                }
            }

            var managerList = new SelectList(_accountService.GetAllManagers(), "Id", "DisplayString");
            ViewBag.managerList = managerList;
            return View(team);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                return View(_teamService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (TeamNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        // POST: Team/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Team team = _teamService.GetDetails(id);

            var result = _teamService.Delete(team);

            if (result.Success)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.SystemError, @Message = "Unable to delete entity." });
            }
        }

        public List<ApplicationUser> GetTeamMembers(int teamId, string managerId)
        {
            var members = new List<ApplicationUser>();

            foreach (var user in _accountService.GetAll())
            {
                if (user.TeamId != null && user.TeamId == teamId && user.Id != managerId)
                {
                    if (!members.Contains(user))
                    {
                        members.Add(user);
                    }
                }
            }

            return members;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _teamService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
