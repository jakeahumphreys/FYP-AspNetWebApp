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
using Microsoft.AspNet.Identity;

namespace FYP_WebApp.Controllers
{
    public class NoteController : Controller
    {
        private readonly NoteService _noteService;

        public NoteController()
        {
            _noteService = new NoteService();
        }

        public NoteController(INoteRepository noteRepository)
        {
            _noteService = new NoteService(noteRepository);
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_noteService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error",
                    new {@Error = Errors.InvalidParameter, @Message = ex.Message});
            }
            catch (NoteNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [CustomAuth(Roles = "Admin, Manager, Member")]
        public ActionResult Create(int storedLocationId)
        {
            var noteViewModel = new NoteViewModel {StoredLocationId = storedLocationId};
            return View(noteViewModel);
        }

        [CustomAuth(Roles = "Admin, Manager, Member")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NoteViewModel noteViewModel)
        {

            if (noteViewModel == null)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = "Invalid Parameter" });
            }

            if (noteViewModel.Note == null)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = "Invalid Parameter" });
            }

            noteViewModel.Note.StoredLocationId = noteViewModel.StoredLocationId;
            noteViewModel.Note.SenderId = User.Identity.GetUserId();
            noteViewModel.Note.TimeCreated = DateTime.Now;

            if (ModelState.IsValid)
            {
              
                var result = _noteService.Create(noteViewModel.Note);

                if (result.Success)
                {
                    return RedirectToAction("Details", "StoredLocation", new {@id = noteViewModel.StoredLocationId});
                }
                else
                {
                    if (result.ResponseError == ResponseErrors.NullParameter)
                    {
                        return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = "Invalid Parameter" });
                    }
                }
            }

            return View(noteViewModel);
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_noteService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error",
                    new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (NoteNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuth(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Title,Content,StoredLocationId,IsInactive")] Note note)
        {
            if (ModelState.IsValid)
            {
                var result = _noteService.Edit(note);

                if (result.Success)
                {
                    return RedirectToAction("Details", "Note", new {@id = note.Id});
                }
                else
                {
                    return View(note);
                }
            }

            return View(note);
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                return View(_noteService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error",
                    new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (NoteNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [CustomAuth(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Note note = _noteService.GetDetails(id);

            var result = _noteService.Delete(note);

            if (result.Success)
            {
                return RedirectToAction("Details", "StoredLocation", new {@id = note.StoredLocationId});
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
                _noteService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
