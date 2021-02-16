using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IStoredLocationRepository : IDisposable
    {
        List<StoredLocation> GetAll();
        StoredLocation GetById(int id);
        void Insert(StoredLocation storedLocation);
        void Update(StoredLocation storedLocation);
        void Delete(StoredLocation storedLocation);
        void Save();
    }
}