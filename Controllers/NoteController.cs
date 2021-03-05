using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;

namespace FYP_WebApp.Controllers
{
    public class NoteController : Controller
    {
        private NoteService _noteService;

        public NoteController()
        {
            _noteService = new NoteService();
        }

        // GET: Note
        public ActionResult Index()
        {
            return View(_noteService.GetAll());
        }

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

        // GET: Note/Create
        public ActionResult Create(int storedLocationId)
        {
            //ViewBag.StoredLocationId = new SelectList(db.StoredLocations, "Id", "Label");
            var noteViewModel = new NoteViewModel {StoredLocationId = storedLocationId};
            return View(noteViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Note, StoredLocationId")] NoteViewModel noteViewModel)
        {
            if (ModelState.IsValid)
            {
                noteViewModel.Note.StoredLocationId = noteViewModel.StoredLocationId;
                var result = _noteService.Create(noteViewModel.Note);

                if (result.Success)
                {
                    return RedirectToAction("Details", "StoredLocation", new {@id = noteViewModel.StoredLocationId});
                }
            }

            return View(noteViewModel);
        }

        // GET: Note/Edit/5
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

        // GET: Note/Delete/5
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

        // POST: Note/Delete/5
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
