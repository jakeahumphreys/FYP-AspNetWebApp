using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;

namespace FYP_WebApp.API
{
    public class StoredLocationController : ApiController
    {

        private readonly StoredLocationService _storedLocationService;
        private Mapper mapper;

        public StoredLocationController()
        {
            _storedLocationService = new StoredLocationService();
            var config = AutomapperConfig.instance().Configure();
            mapper = new Mapper(config);
        }

        [HttpGet]
        public IEnumerable<StoredLocationDto> Get()
        {
            return mapper.Map<IList<StoredLocation>, List<StoredLocationDto>>(_storedLocationService.Index());
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var storedLocation = _storedLocationService.GetDetails(id);

            if (storedLocation != null)
            {
                return Json(mapper.Map<StoredLocation, StoredLocationDto>(storedLocation));
            }
            else
            {
                return Content(HttpStatusCode.NotFound, "A location with the specified ID was not found.");
            }
        }
    }
}
