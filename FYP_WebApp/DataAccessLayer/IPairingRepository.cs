using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IPairingRepository : IDisposable
    {
        List<Pairing> GetAll();
        Pairing GetById(int id);
        void Insert(Pairing pairing);
        void Update(Pairing pairing);
        void Delete(Pairing pairing);
        void Save();
    }
}