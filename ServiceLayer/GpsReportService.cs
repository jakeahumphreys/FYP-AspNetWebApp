using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;

namespace FYP_WebApp.ServiceLayer
{
    public class GpsReportService
    {
        private readonly IGpsReportRepository _gpsReportRepository;

        public GpsReportService()
        {
            _gpsReportRepository = new GpsReportRepository();
        }

        public GpsReportService(IGpsReportRepository gpsReportRepository)
        {
            _gpsReportRepository = gpsReportRepository;
        }

        public List<GpsReport> GetAll()
        {
            return _gpsReportRepository.GetAll();
        }

        public GpsReport Details(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified");
            }

            var gpsReport = _gpsReportRepository.GetById(id);

            if (gpsReport == null)
            {
                throw new GpsReportNotFoundException("A GPS Report with ID " + id + " was not found.");
            }

            return gpsReport;
        }

        public ServiceResponse Create(GpsReport gpsReport)
        {
            if (gpsReport == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                _gpsReportRepository.Insert(gpsReport);
                _gpsReportRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public ServiceResponse Edit(GpsReport gpsReport)
        {
            if (gpsReport == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                var existingReport = _gpsReportRepository.GetById(gpsReport.Id);

                existingReport.LocationId = gpsReport.LocationId;

                _gpsReportRepository.Update(existingReport);
                _gpsReportRepository.Save();

                return new ServiceResponse { Success = true };
            }
        }

        public List<GpsReport> GetGpsReports(List<ApplicationUser> users)
        {
            var gpsReports = new List<GpsReport>();

            foreach (var user in users)
            {
                if (_gpsReportRepository.GetAll().Count(x => x.UserId == user.Id) > 0)
                {
                    gpsReports.Add(_gpsReportRepository.GetAll().Where(x => x.UserId == user.Id).OrderByDescending(e => e.Time).FirstOrDefault());
                }
            }

            return gpsReports;
        }

        public void LinkExistingLocations(StoredLocation storedLocation)
        {
            if (storedLocation != null)
            {
                foreach (var report in _gpsReportRepository.GetAll())
                {
                    if (report.Latitude == storedLocation.Latitude && report.Longitude == storedLocation.Longitude &&
                        report.LocationId == null)
                    {
                        var existingReport = _gpsReportRepository.GetById(report.Id);
                        existingReport.LocationId = storedLocation.Id;
                        _gpsReportRepository.Update(existingReport);
                        _gpsReportRepository.Save();
                    }
                }
            }
        }

        public void Dispose()
        {
            _gpsReportRepository.Dispose();
        }
    }
}