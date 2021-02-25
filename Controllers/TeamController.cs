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

namespace FYP_WebApp.Controllers
{
    public class TeamController : Controller
    {
        private TeamService _teamService;

        public TeamController()
        {
            _teamService = new TeamService();
        }

        public ActionResult Index()
        {
            return View(_teamService.GetAll());
        }

        public ActionResult Details(int id)
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

        public ActionResult Create()
        {
            return View();
        }

        // POST: Team/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,IsInactive")] Team team)
        {
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

        // GET: Team/Edit/5
        public ActionResult Edit(int id)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,IsInactive")] Team team)
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
