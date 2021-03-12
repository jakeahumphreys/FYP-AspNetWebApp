using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IAccessLogRepository : IDisposable
    {
        List<AccessLog> GetAll();
        AccessLog GetById(int id);
        void Insert(AccessLog accessLog);
        void Update(AccessLog accessLog);
        void Delete(AccessLog accessLog);
        void Save();
    }
}