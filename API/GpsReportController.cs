using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Spatial;
using System.Device.Location;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
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
        private readonly StoredLocationService _storedLocationService;
        private readonly ConfigurationRecordService _configurationRecordService;
        private readonly LogService _logService;
        private Mapper _mapper;


        public GpsReportController()
        {
            _logService = new LogService();
            _gpsReportService = new GpsReportService();
            _storedLocationService = new StoredLocationService();
            _configurationRecordService = new ConfigurationRecordService();
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

                var reportedLocation = new GeoCoordinate
                    { Latitude = Convert.ToDouble(gpsReport.Latitude), Longitude = Convert.ToDouble(gpsReport.Longitude) };

                var nearestLocation = GetClosestLocation(reportedLocation);

                var nearestLocationGeo = new GeoCoordinate(Convert.ToDouble(nearestLocation.Latitude),
                    Convert.ToDouble(nearestLocation.Longitude));

                double locationAccuracy;

                var configRecord = _configurationRecordService.GetLatestConfigurationRecord();

                if (configRecord != null)
                {
                    locationAccuracy = configRecord.StoreLocationAccuracy;
                }
                else
                {
                    locationAccuracy = 25;
                }

                var distance = reportedLocation.GetDistanceTo(nearestLocationGeo);

                if (distance <= locationAccuracy)
                {
                    gpsReport.LocationId = nearestLocation.Id;
                }

                var result = _gpsReportService.Create(gpsReport);

                if (result.Success)
                {

                    var response = Content(HttpStatusCode.OK, "GPS Reported Successfully");

                    var additionalFields = new List<ApiLogAdditionalField>();
                    additionalFields.Add(new ApiLogAdditionalField{Key = "Distance", Value = distance.ToString()});

                    _logService.CreateApiLog(new ApiLog
                    {
                        LogLevel = LogLevel.Info,
                        Controller = this.ControllerContext.ControllerDescriptor.ControllerName,
                        Action = this.ActionContext.ActionDescriptor.ActionName,
                        TimeStamp = DateTime.Now,
                        RequestString = JsonConvert.SerializeObject(request,Formatting.Indented),
                        ResponseString = JsonConvert.SerializeObject(response.Content, Formatting.Indented),
                        StatusCode = HttpStatusCode.OK.ToString(),
                        AdditionalFields = JsonConvert.SerializeObject(additionalFields)
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

        public StoredLocation GetClosestLocation(GeoCoordinate reportedLocation)
        {
            
            var nearestLocation = (from location in _storedLocationService.Index()
                let geo = new GeoCoordinate
                    {Latitude = Convert.ToDouble(location.Latitude), Longitude = Convert.ToDouble(location.Longitude)}
                orderby geo.GetDistanceTo(reportedLocation)
                select location).FirstOrDefault();

            return nearestLocation;
        }
    }
}
