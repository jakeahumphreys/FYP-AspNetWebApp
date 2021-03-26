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

namespace FYP_WebApp.Controllers
{
    public class ConfigurationRecordController : Controller
    {
        private readonly ConfigurationRecordService _configurationRecordService;

        public ConfigurationRecordController()
        {
            _configurationRecordService = new ConfigurationRecordService();
        }

        public ConfigurationRecordController(IConfigurationRecordRepository configRecordRepository)
        {
            _configurationRecordService = new ConfigurationRecordService(configRecordRepository);
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Index()
        {
            return View(_configurationRecordService.GetAll());
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Details(int id)
        {
            try
            {
                return View(_configurationRecordService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (ConfigurationRecordNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ConfigurationRecord configurationRecord)
        {
            if (ModelState.IsValid)
            {
                var result = _configurationRecordService.Create(configurationRecord);

                if (result.Success)
                {
                    return Redirect("Index");
                }
                else
                {
                    return View(configurationRecord);
                }
            }

            return View(configurationRecord);
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            try
            {
                return View(_configurationRecordService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (ConfigurationRecordNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ConfigurationRecord configurationRecord)
        {
            if (ModelState.IsValid)
            {
                var result = _configurationRecordService.Edit(configurationRecord);

                if (result.Success)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View(configurationRecord);
                }
            }

            return View(configurationRecord);
        }

        [CustomAuth(Roles = "Admin")]
        public ActionResult Delete(int id)
        {
            try
            {
                return View(_configurationRecordService.GetDetails(id));
            }
            catch (ArgumentException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.InvalidParameter, @Message = ex.Message });
            }
            catch (ConfigurationRecordNotFoundException ex)
            {
                return RedirectToAction("Error", "Error", new { @Error = Errors.EntityNotFound, @Message = ex.Message });
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ConfigurationRecord configurationRecord = _configurationRecordService.GetDetails(id);

            var result = _configurationRecordService.Delete(configurationRecord);

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
                _configurationRecordService.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
