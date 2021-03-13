using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IConfigurationRecordRepository : IDisposable
    {
        List<ConfigurationRecord> GetAll();
        ConfigurationRecord GetById(int id);
        void Insert(ConfigurationRecord configurationRecord);
        void Update(ConfigurationRecord configurationRecord);
        void Delete(ConfigurationRecord configurationRecord);
        void Save();
    }
}