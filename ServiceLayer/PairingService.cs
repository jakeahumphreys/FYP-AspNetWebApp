using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Microsoft.Owin.Security;

namespace FYP_WebApp.ServiceLayer
{
    public class PairingService
    {
        private readonly IPairingRepository _pairingRepository;
        private readonly Library _library;

        public PairingService()
        {
            _pairingRepository = new PairingRepository(new ApplicationDbContext());
            _library = new Library();
        }

        public PairingService(IPairingRepository pairingRepository)
        {
            _pairingRepository = pairingRepository;
            _library = new Library();

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

        public List<Pairing> CheckConflictingPairings(Pairing pairing)
        {
            var conflictingPairings = new List<Pairing>();

            if (pairing != null)
            {
                foreach (var existingPairing in _pairingRepository.GetAll())
                {
                    if (existingPairing.Id != pairing.Id)
                    {
                        if (existingPairing.UserId == pairing.UserId || existingPairing.BuddyUserId == pairing.BuddyUserId)
                        {
                            if (pairing.Start <= existingPairing.End && existingPairing.Start <= pairing.End)
                            {
                                conflictingPairings.Add(existingPairing);
                            }
                        }
                    }
                  
                }
            }

            return conflictingPairings;
        }

        public PairingViewModel ConstructViewModel(Pairing pairing, SelectList userList)
        {
            var deconstructedStartDate = _library.DeconstructDateTime(pairing.Start);
            var deconstructedEndDate = _library.DeconstructDateTime(pairing.End);

            return new PairingViewModel
            {
                Pairing = pairing,
                StartDate = deconstructedStartDate[0],
                StartTime = deconstructedStartDate[1],
                EndDate = deconstructedEndDate[0],
                EndTime = deconstructedEndDate[1],
                UserList = userList
            };
        }

        public List<ApplicationUser> GetDailyUnpairedUsers(List<ApplicationUser> teamStaff)
        {
            if (teamStaff == null)
            {
                return null;
            }

            var unpairedUsers = new List<ApplicationUser>();

            foreach (var user in teamStaff)
            {
                List<Pairing> pairings = _pairingRepository.GetAll().Where(x =>
                    x.UserId == user.Id && x.Start >= DateTime.Today && x.End >= DateTime.Today).ToList();

                if (pairings.Count == 0)
                {
                    unpairedUsers.Add(user);
                }
            }

            return unpairedUsers;
        }
    }
}