using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Management;
using System.Web.Mvc;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.Controllers
{
    public class StoredLocationController : Controller
    {
        private readonly StoredLocationService _storedLocationService;

        public StoredLocationController()
        {
            _storedLocationService = new StoredLocationService();
        }

        public ActionResult Index()
        {
            return View(_storedLocationService.Index());
        }

        public ActionResult Details(int id)
        {
            try
            {
                return View(_storedLocationService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.AccountInactive, @Message = ex.Message });
            }
            catch (StoredLocationNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.AccountInactive, @Message = ex.Message });
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Label,Latitude,Longitude")] StoredLocation storedLocation)
        {
            if (ModelState.IsValid)
            {
                ServiceResponse response = _storedLocationService.CreateAction(storedLocation);

                if (response.Success == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(storedLocation);
                }
            }

            return View(storedLocation);
        }

        public ActionResult Edit(int id)
        {
            try
            {
                return View(_storedLocationService.EditView(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.AccountInactive, @Message = ex.Message });
            }
            catch (StoredLocationNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.AccountInactive, @Message = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Label,Latitude,Longitude,IsInactive")] StoredLocation storedLocation)
        {

            if (ModelState.IsValid)
            {
                ServiceResponse response = _storedLocationService.EditAction(storedLocation);

                if (response.Success == true)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(storedLocation);
                }
            }

            return View(storedLocation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _storedLocationService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
