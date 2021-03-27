using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IGpsReportRepository : IDisposable
    {
        List<GpsReport> GetAll();
        GpsReport GetById(int id);
        void Insert(GpsReport gpsReport);
        void Update(GpsReport gpsReport);
        void Delete(GpsReport gpsReport);
        void Save();
    }
}