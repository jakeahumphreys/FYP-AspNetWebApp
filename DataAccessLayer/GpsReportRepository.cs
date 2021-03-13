using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;
using Microsoft.Ajax.Utilities;

namespace FYP_WebApp.DataAccessLayer
{
    public class GpsReportRepository : IGpsReportRepository
    {
        private readonly ApplicationDbContext _context;

        public GpsReportRepository()
        {
            _context = new ApplicationDbContext();
        }

        public GpsReportRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<GpsReport> GetAll()
        {
            return _context.GpsReports.Include(x=> x.User).Include(x=> x.Location).ToList();
        }

        public GpsReport GetById(int id)
        {
            return _context.GpsReports.Where(x => x.Id == id).Include(x=>x.User).Include(x => x.Location).SingleOrDefault();
        }

        public void Insert(GpsReport gpsReport)
        {
            _context.GpsReports.Add(gpsReport);
        }

        public void Update(GpsReport gpsReport)
        {
            _context.Entry(gpsReport).State = EntityState.Modified;
        }

        public void Delete(GpsReport gpsReport)
        {
            _context.GpsReports.Remove(gpsReport);
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}