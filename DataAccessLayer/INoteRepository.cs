using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface INoteRepository : IDisposable
    {
        List<Note> GetAll();
        Note GetById(int id);
        void Insert(Note note);
        void Update(Note note);
        void Delete(Note note);
        void Save();
    }
}