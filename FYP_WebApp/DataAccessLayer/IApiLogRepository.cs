using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IApiLogRepository : IDisposable
    {
        List<ApiLog> GetAll();
        ApiLog GetById(int id);
        void Insert(ApiLog apiLog);
        void Update(ApiLog apiLog);
        void Delete(ApiLog apiLog);
        void Save();
    }
}