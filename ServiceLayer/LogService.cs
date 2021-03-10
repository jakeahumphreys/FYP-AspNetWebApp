using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;

namespace FYP_WebApp.ServiceLayer
{
    public class LogService
    {
        private readonly ApiLogRepository _apiLogRepository;

        public LogService()
        {
            _apiLogRepository = new ApiLogRepository();
        }

        public List<ApiLog> GetAllApiLogs()
        {
            return _apiLogRepository.GetAll();
        }

        public ApiLog GetApiLogDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified.");
            }

            var apiLog = _apiLogRepository.GetById(id);

            if (apiLog == null)
            {
                throw new ApiLogNotFoundException($"API Log with ID {id} was not found.");
            }

            return apiLog;
        }

        public ServiceResponse CreateApiLog(ApiLog apiLog)
        {
            if (apiLog != null)
            {
                _apiLogRepository.Insert(apiLog);
                _apiLogRepository.Save();
                return new ServiceResponse { Success = true };
            }
            else
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter};
            }
        }

        public void Dispose()
        {
            _apiLogRepository.Dispose();
        }
    }
}