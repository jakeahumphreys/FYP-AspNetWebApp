using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class StoredLocationRepository : IStoredLocationRepository
    {
        private readonly ApplicationDbContext _context;

        public StoredLocationRepository()
        {
            _context = new ApplicationDbContext();
        }

        public StoredLocationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<StoredLocation> GetAll()
        {
            return _context.StoredLocations.Include(x=> x.Notes.Select(y=> y.Sender)).Include(x => x.CheckIns).ToList();
        }

        public StoredLocation GetById(int id)
        {
            return _context.StoredLocations.Where(x => x.Id == id).Include(x => x.Notes.Select(y => y.Sender)).Include(x => x.CheckIns).SingleOrDefault();
        }

        public void Insert(StoredLocation storedLocation)
        {
            _context.StoredLocations.Add(storedLocation);
        }

        public void Update(StoredLocation storedLocation)
        {
            _context.Entry(storedLocation).State = EntityState.Modified;
        }

        public void Delete(StoredLocation storedLocation)
        {
            _context.StoredLocations.Remove(storedLocation);
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