using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Host.SystemWeb;

namespace FYP_WebApp.API
{


    public class AccountController : ApiController
    {

        private readonly ApplicationDbContext _applicationDbContext;
        private  readonly Mapper _mapper;

        public AccountController()
        {
            _applicationDbContext = new ApplicationDbContext();
            var config = AutomapperConfig.instance().Configure();
            _mapper = new Mapper(config);
        }

        [HttpPost]
        [Route("api/Account/Login")]
        public IHttpActionResult Login(string username, string loginKey)
        {
            var user = _applicationDbContext.Users
                .FirstOrDefault(x => x.UserName == username && x.MobileLoginKey == loginKey);

            if (user == null)
            {
                return Content(HttpStatusCode.NotFound, "No user matched the specified parameters.");
            }
            else
            {
                return Json(_mapper.Map<ApplicationUser, MobileUserDto>(user));
            }
        }
    }
}
