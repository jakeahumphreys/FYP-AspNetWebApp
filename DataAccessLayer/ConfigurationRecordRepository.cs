using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class ConfigurationRecordRepository : IConfigurationRecordRepository
    {
        private readonly ApplicationDbContext _context;

        public ConfigurationRecordRepository()
        {
            _context = new ApplicationDbContext();
        }

        public ConfigurationRecordRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ConfigurationRecord> GetAll()
        {
            return _context.ConfigurationRecords.ToList();
        }

        public ConfigurationRecord GetById(int id)
        {
            return _context.ConfigurationRecords.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(ConfigurationRecord configurationRecord)
        {
            _context.ConfigurationRecords.Add(configurationRecord);
        }

        public void Update(ConfigurationRecord configurationRecord)
        {
            _context.Entry(configurationRecord).State = EntityState.Modified;
        }

        public void Delete(ConfigurationRecord configurationRecord)
        {
            _context.ConfigurationRecords.Remove(configurationRecord);
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