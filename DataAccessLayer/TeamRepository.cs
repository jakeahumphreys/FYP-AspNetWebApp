using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public class TeamRepository : ITeamRepository
    {
        private readonly ApplicationDbContext _context;

        public TeamRepository()
        {
            _context = new ApplicationDbContext();
        }

        public TeamRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Team> GetAll()
        {
            return _context.Teams.ToList();
        }

        public Team GetById(int id)
        {
            return _context.Teams.SingleOrDefault(x => x.Id == id);
        }

        public void Insert(Team team)
        {
            _context.Teams.Add(team);
        }

        public void Update(Team team)
        {
            _context.Entry(team).State = EntityState.Modified;
        }

        public void Delete(Team team)
        {
            _context.Teams.Remove(team);
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