﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Management;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.ServiceLayer
{
    public class TeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService()
        {
            _teamRepository = new TeamRepository(new ApplicationDbContext());
        }

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public List<Team> GetAll()
        {
            return _teamRepository.GetAll();
        }

        public Team GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified.");
            }

            var team = _teamRepository.GetById(id);

            if (team == null)
            {
                throw new TeamNotFoundException("A team with ID " + id + " was not found.");
            }

            return team;
        }

        public ServiceResponse Create(Team team)
        {
            if (team == null)
            {
                return new ServiceResponse { Success = false };
            }
            else
            {
                _teamRepository.Insert(team);
                _teamRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public ServiceResponse Edit(Team team)
        {
            if (team == null)
            {
                return new ServiceResponse { Success = false };
            }
            else
            {
                var existingTeam = _teamRepository.GetById(team.Id);
                existingTeam.Name = team.Name;
                existingTeam.ManagerId = team.ManagerId;

                _teamRepository.Update(existingTeam);
                _teamRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public ServiceResponse Delete(Team team)
        {
            if (team == null)
            {
                return new ServiceResponse {Success = false};
            }
            else
            {
                _teamRepository.Delete(team);
                _teamRepository.Save();
                return new ServiceResponse {Success = true};
            }
        }

        public void Dispose()
        {
            _teamRepository.Dispose();
        }

        public List<ApplicationUser> CheckAvailableManagers(List<ApplicationUser> managers, int teamId)
        {
            var availableManagers = new List<ApplicationUser>();

            foreach (var manager in managers)
            {
                foreach (var team in _teamRepository.GetAll())
                {
                    if (team.Id != teamId)
                    {
                        if (team.ManagerId == manager.Id)
                        {
                            break;
                        }

                        if (!availableManagers.Contains(manager))
                        {
                            availableManagers.Add(manager);
                        }
                    }
                }
            }

            return availableManagers;
        }

        public List<Team> GetUserTeams(string id)
        {
            var userTeams = new List<Team>();

            foreach (var team in _teamRepository.GetAll())
            {
                if (team.ManagerId == id)
                {
                    userTeams.Add(team);
                }
            }

            return userTeams;
        }
    }
}