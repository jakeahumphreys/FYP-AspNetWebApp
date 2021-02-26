using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class PairingRepository: IPairingRepository
    {
        private readonly ApplicationDbContext _context;

        public PairingRepository()
        {
            _context = new ApplicationDbContext();
        }

        public PairingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Pairing> GetAll()
        {
            return _context.Pairings.Include(x=> x.User).Include(x => x.BuddyUser).ToList();
        }

        public Pairing GetById(int id)
        {
            return _context.Pairings.Where(x => x.Id == id).Include(x => x.User).Include(x => x.BuddyUser).SingleOrDefault();
        }

        public void Insert(Pairing pairing)
        {
            _context.Pairings.Add(pairing);
        }

        public void Update(Pairing pairing)
        {
            _context.Entry(pairing).State = EntityState.Modified;
        }

        public void Delete(Pairing pairing)
        {
            _context.Pairings.Remove(pairing);
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