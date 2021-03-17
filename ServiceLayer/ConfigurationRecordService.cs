using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;

namespace FYP_WebApp.ServiceLayer
{
    public class ConfigurationRecordService
    {
        private readonly IConfigurationRecordRepository _configurationRecordRepository;

        public ConfigurationRecordService()
        {
            _configurationRecordRepository = new ConfigurationRecordRepository(new ApplicationDbContext());
        }

        public ConfigurationRecordService(IConfigurationRecordRepository configurationRecordRepository)
        {
            _configurationRecordRepository = configurationRecordRepository;
        }

        public List<ConfigurationRecord> GetAll()
        {
            return _configurationRecordRepository.GetAll();
        }

        public ConfigurationRecord GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified.");
            }

            var configurationRecord = _configurationRecordRepository.GetById(id);

            if (configurationRecord == null)
            {
                throw new ConfigurationRecordNotFoundException("A Configuration Record with ID " + id + " was not found.");
            }

            return configurationRecord;
        }

        public ServiceResponse Create(ConfigurationRecord configurationRecord)
        {
            if (configurationRecord == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                configurationRecord.Created = DateTime.Now;
                _configurationRecordRepository.Insert(configurationRecord);
                _configurationRecordRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public ServiceResponse Edit(ConfigurationRecord configurationRecord)
        {
            if (configurationRecord == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                var existingConfigurationRecord = _configurationRecordRepository.GetById(configurationRecord.Id);
                existingConfigurationRecord.Created = DateTime.Now;
                existingConfigurationRecord.StoreLocationAccuracy = configurationRecord.StoreLocationAccuracy;
                existingConfigurationRecord.MessageOfTheDayText = configurationRecord.MessageOfTheDayText;
                existingConfigurationRecord.SmtpSendUrgentEmails = configurationRecord.SmtpSendUrgentEmails;
                existingConfigurationRecord.SmtpUrl = configurationRecord.SmtpUrl;
                existingConfigurationRecord.SmtpPort = configurationRecord.SmtpPort;
                existingConfigurationRecord.SmtpSenderUsername = configurationRecord.SmtpSenderUsername;
                existingConfigurationRecord.SmtpSenderPassword = configurationRecord.SmtpSenderPassword;
                existingConfigurationRecord.SmtpShouldUseSsl = configurationRecord.SmtpShouldUseSsl;
                existingConfigurationRecord.SmtpEmailFrom = configurationRecord.SmtpEmailFrom;
                existingConfigurationRecord.MapsApiKey = configurationRecord.MapsApiKey;

                _configurationRecordRepository.Update(existingConfigurationRecord);
                _configurationRecordRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public ServiceResponse Delete(ConfigurationRecord configurationRecord)
        {
            if (configurationRecord == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                _configurationRecordRepository.Delete(configurationRecord);
                _configurationRecordRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public ConfigurationRecord GetLatestConfigurationRecord()
        {
            return _configurationRecordRepository.GetAll().OrderByDescending(x => x.Created).FirstOrDefault();
        }

        public void Dispose()
        {
            _configurationRecordRepository.Dispose();
        }
    }
}