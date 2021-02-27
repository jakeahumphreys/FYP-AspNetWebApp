using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.Models;

namespace FYP_WebApp.ServiceLayer
{
    public class AccountService
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public AccountService()
        {
            _applicationDbContext = new ApplicationDbContext();
        }

        public List<ApplicationUser> GetAll()
        {
            return _applicationDbContext.Users.ToList();
        }

        public ApplicationUser GetById(string id)
        {
            return _applicationDbContext.Users.Find(id);
        }

        public List<ApplicationUser> GetAllManagers()
        {
            var managerRole = _applicationDbContext.Roles.SingleOrDefault(r => r.Name == "Manager");
            return _applicationDbContext.Users
                .Where(user => user.Roles.Any(r => r.RoleId == managerRole.Id)).ToList();
        }

        public ApplicationUser GetDetails(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("No ID specified.");
            }

            var user = _applicationDbContext.Users.Find(id);

            if (user == null)
            {
                throw new UserNotFoundException("A user with ID " + id + " was not found.");
            }

            return user;
        }

        public ServiceResponse SetUserStatus(string userId, Status status)
        {
            if (userId == null || status == null)
            {
                return new ServiceResponse {Success = false, ResponseError = ResponseErrors.NullParameter};
            }

            var user = _applicationDbContext.Users.Find(userId);

            if (user == null)
            {
                return new ServiceResponse {Success = false, ResponseError = ResponseErrors.EntityNotFound};
            }

            user.Status = status;

            _applicationDbContext.Entry(user).State = EntityState.Modified;
            _applicationDbContext.SaveChanges();

            return new ServiceResponse {Success = true};
        }
    }
}