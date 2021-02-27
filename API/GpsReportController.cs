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
    public class GpsReportController : ApiController
    {
        private readonly GpsReportService _gpsReportService;
        private Mapper _mapper;


        public GpsReportController()
        {
            _gpsReportService = new GpsReportService();
            var config = AutomapperConfig.instance().Configure();
            _mapper = new Mapper(config);
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] GpsReportDto request)
        {
            if (request == null)
            {
                return Content(HttpStatusCode.BadRequest, "Request was null.");
            }
            else
            {
                var gpsReport = _mapper.Map<GpsReportDto, GpsReport>(request);
                var result = _gpsReportService.Create(gpsReport);

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, "GPS Reported Successfully");
                }
                else
                {
                    switch (result.ResponseError)
                    {
                        case ResponseErrors.NullParameter:
                            return Content(HttpStatusCode.BadRequest, "One of the specified parameters was null or invalid.");
                        default:
                            return BadRequest();
                    }
                }
            }
        }
    }
}
