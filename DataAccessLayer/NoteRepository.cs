using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;

        public NoteRepository()
        {
            _context = new ApplicationDbContext();
        }

        public NoteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Note> GetAll()
        {
            return _context.Notes.ToList();
        }

        public Note GetById(int id)
        {
            return _context.Notes.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(Note note)
        {
            _context.Notes.Add(note);
        }

        public void Update(Note note)
        {
            _context.Entry(note).State = EntityState.Modified;
        }

        public void Delete(Note note)
        {
            _context.Notes.Remove(note);
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