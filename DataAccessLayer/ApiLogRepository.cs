using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class ApiLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ApiLogRepository()
        {
            _context = new ApplicationDbContext();
        }

        public ApiLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ApiLog> GetAll()
        {
            return _context.ApiLogs.ToList();
        }

        public ApiLog GetById(int id)
        {
            return _context.ApiLogs.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(ApiLog apiLog)
        {
            _context.ApiLogs.Add(apiLog);
        }

        public void Update(ApiLog apiLog)
        {
            _context.Entry(apiLog).State = EntityState.Modified;
        }

        public void Delete(ApiLog apiLog)
        {
            _context.ApiLogs.Remove(apiLog);
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