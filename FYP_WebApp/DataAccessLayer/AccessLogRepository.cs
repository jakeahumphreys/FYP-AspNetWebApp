using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class AccessLogRepository : IAccessLogRepository
    {
        private readonly ApplicationDbContext _context;

        public AccessLogRepository()
        {
            _context = new ApplicationDbContext();
        }

        public AccessLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<AccessLog> GetAll()
        {
            return _context.AccessLogs.ToList();
        }

        public AccessLog GetById(int id)
        {
            return _context.AccessLogs.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(AccessLog accessLog)
        {
            _context.AccessLogs.Add(accessLog);
        }

        public void Update(AccessLog accessLog)
        {
            _context.Entry(accessLog).State = EntityState.Modified;
        }

        public void Delete(AccessLog accessLog)
        {
            _context.AccessLogs.Remove(accessLog);
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