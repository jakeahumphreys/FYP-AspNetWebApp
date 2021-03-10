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
    public class GpsReportController : ApiController
    {
        private readonly GpsReportService _gpsReportService;
        private readonly LogService _logService;
        private Mapper _mapper;


        public GpsReportController()
        {
            _logService = new LogService();
            _gpsReportService = new GpsReportService();
            var config = AutomapperConfig.instance().Configure();
            _mapper = new Mapper(config);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] GpsReportDto request)
        {
            if (request == null)
            {
                var response = Content(HttpStatusCode.BadRequest, "Request was null.");

                _logService.CreateApiLog(new ApiLog
                {
                    LogLevel = LogLevel.Error,
                    Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                    Action = this.ActionContext.ActionDescriptor.ActionName,
                    TimeStamp = DateTime.Now,
                    RequestString = "NULL",
                    ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                    StatusCode = HttpStatusCode.BadRequest.ToString()
                });

                return response;
            }
            else
            {
                var gpsReport = _mapper.Map<GpsReportDto, GpsReport>(request);
                var result = _gpsReportService.Create(gpsReport);

                if (result.Success)
                {
                    var response = Content(HttpStatusCode.OK, "GPS Reported Successfully");

                    _logService.CreateApiLog(new ApiLog
                    {
                        LogLevel = LogLevel.Info,
                        Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                        Action = this.ActionContext.ActionDescriptor.ActionName,
                        TimeStamp = DateTime.Now,
                        RequestString = JsonConvert.SerializeObject(request,Formatting.Indented),
                        ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                        StatusCode = HttpStatusCode.OK.ToString()
                    });

                    return response;
                }
                else
                {
                    switch (result.ResponseError)
                    {
                        case ResponseErrors.NullParameter:
                            var response = Content(HttpStatusCode.BadRequest, "One of the specified parameters was null or invalid.");

                            _logService.CreateApiLog(new ApiLog
                            {
                                LogLevel = LogLevel.Error,
                                Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                                Action = this.ActionContext.ActionDescriptor.ActionName,
                                TimeStamp = DateTime.Now,
                                RequestString = JsonConvert.SerializeObject(request, Formatting.Indented),
                                ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                                StatusCode = HttpStatusCode.BadRequest.ToString()
                            });

                            return response;
                        default:
                            return BadRequest();
                    }
                }
            }
        }
    }
}
