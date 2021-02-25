using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface ITeamRepository : IDisposable
    {
        List<Team> GetAll();
        Team GetById(int id);
        void Insert(Team team);
        void Update(Team team);
        void Delete(Team team);
        void Save();
    }
}