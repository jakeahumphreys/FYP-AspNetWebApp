using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _context;

        public MessageRepository()
        {
            _context = new ApplicationDbContext();
        }

        public MessageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Message> GetAll()
        {
            return _context.Messages.Include(x => x.Sender).Include(x=> x.Recipient).ToList();
        }

        public Message GetById(int id)
        {
            return _context.Messages.Where(x => x.Id == id).Include(x=> x.Sender).Include(x=> x.Recipient).SingleOrDefault();
        }

        public void Insert(Message message)
        {
            _context.Messages.Add(message);
        }

        public void Update(Message message)
        {
            _context.Entry(message).State = EntityState.Modified;
        }

        public void Delete(Message message)
        {
            _context.Messages.Remove(message);
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