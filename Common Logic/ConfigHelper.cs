using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;

namespace FYP_WebApp.Common_Logic
{
    public class ConfigHelper
    {
        private static readonly ConfigurationRecordService
            ConfigurationRecordService = new ConfigurationRecordService();


        public static ConfigurationRecord GetLatestConfigRecord()
        {
            return ConfigurationRecordService.GetLatestConfigurationRecord();
        }
    }
}