using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Microsoft.Owin.Security;

namespace FYP_WebApp.ServiceLayer
{
    public class PairingService
    {
        private readonly IPairingRepository _pairingRepository;

        public PairingService()
        {
            _pairingRepository = new PairingRepository(new ApplicationDbContext());
        }

        public PairingService(IPairingRepository pairingRepository)
        {
            _pairingRepository = pairingRepository;
        }

        public List<Pairing> GetAll()
        {
            return _pairingRepository.GetAll();
        }

        public Pairing GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified");
            }

            var pairing = _pairingRepository.GetById(id);

            if (pairing == null)
            {
                throw new PairingNotFoundException("A pairing with ID " + id + " was not found.");
            }

            return pairing;
        }

        public ServiceResponse Create(Pairing pairing)
        {
            if (pairing == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                _pairingRepository.Insert(pairing);
                _pairingRepository.Save();
                return new ServiceResponse {Success = true};
            }
        }

        public ServiceResponse Edit(Pairing pairing)
        {
            if (pairing == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                var existingPairing = _pairingRepository.GetById(pairing.Id);
                existingPairing.Start = pairing.Start;
                existingPairing.End = pairing.End;
                existingPairing.UserId = pairing.UserId;
                existingPairing.BuddyUserId = pairing.BuddyUserId;

                _pairingRepository.Update(existingPairing);
                _pairingRepository.Save();
                return new ServiceResponse {Success = true};
            }
        }

        public ServiceResponse Delete(Pairing pairing)
        {
            if (pairing == null)
            {
                return new ServiceResponse {Success = false, ResponseError = ResponseErrors.NullParameter};
            }
            else
            {
                _pairingRepository.Delete(pairing);
                _pairingRepository.Save();
                return new ServiceResponse {Success = true};
            }
        }

        public void Dispose()
        {
            _pairingRepository.Dispose();
        }
    }
}