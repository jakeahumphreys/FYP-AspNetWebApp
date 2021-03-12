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
        private readonly AccessLogRepository _accessLogRepository;

        public LogService()
        {
            _apiLogRepository = new ApiLogRepository();
            _accessLogRepository = new AccessLogRepository();
        }

        public List<ApiLog> GetAllApiLogs()
        {
            return _apiLogRepository.GetAll();
        }

        public List<AccessLog> GetAllAccessLogs()
        {
            return _accessLogRepository.GetAll();
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

        public AccessLog GetAccessLogDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified.");
            }

            var accessLog = _accessLogRepository.GetById(id);

            if (accessLog == null)
            {
                throw new AccessLogNotFoundException($"Access Log with ID {id} was not found.");
            }

            return accessLog;
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

        public ServiceResponse CreateAccessLog(AccessLog accessLog)
        {
            if (accessLog != null)
            {
                _accessLogRepository.Insert(accessLog);
                _accessLogRepository.Save();
                return new ServiceResponse { Success = true };
            }
            else
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
        }

        public void Dispose()
        {
            _apiLogRepository.Dispose();
        }
    }
}