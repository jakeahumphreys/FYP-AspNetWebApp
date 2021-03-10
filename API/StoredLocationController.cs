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
using Newtonsoft.Json;

namespace FYP_WebApp.API
{
    public class StoredLocationController : ApiController
    {

        private readonly StoredLocationService _storedLocationService;
        private readonly LogService _logService;
        private Mapper mapper;

        public StoredLocationController()
        {
            _storedLocationService = new StoredLocationService();
            _logService = new LogService();
            var config = AutomapperConfig.instance().Configure();
            mapper = new Mapper(config);
        }

        [HttpGet]
        public IEnumerable<StoredLocationDto> Get()
        {
            var response =  mapper.Map<IList<StoredLocation>, List<StoredLocationDto>>(_storedLocationService.Index());

            _logService.CreateApiLog(new ApiLog
            {
                LogLevel = LogLevel.Info,
                Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                Action = this.ActionContext.ActionDescriptor.ActionName,
                TimeStamp = DateTime.Now,
                RequestString = JsonConvert.SerializeObject("None", Formatting.Indented),
                ResponseString = JsonConvert.SerializeObject(response, Formatting.Indented),
                StatusCode = HttpStatusCode.OK.ToString()
            });

            return response;
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var storedLocation = _storedLocationService.GetDetails(id);

            if (storedLocation != null)
            {
                var response = Json(mapper.Map<StoredLocation, StoredLocationDto>(storedLocation));

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Info,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject("None", Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.OK.ToString()
                });

                return response;
            }
            else
            {
                var response = Content(HttpStatusCode.NotFound, "A location with the specified ID was not found.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = JsonConvert.SerializeObject("None", Formatting.Indented),
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.NotFound.ToString()
                });

                return response;
            }
        }
    }
}
