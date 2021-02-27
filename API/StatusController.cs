using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.ServiceLayer;

namespace FYP_WebApp.API
{
    public class StatusController : ApiController
    {
        private readonly AccountService _accountService;

        public StatusController()
        {
            _accountService = new AccountService();
        }

        [HttpPost]
        public IHttpActionResult Post([FromBody] StatusDto request)
        {
            if (request == null)
            {
                return Content(HttpStatusCode.BadRequest, "Invalid data submitted");
            }
            else
            {
                var result = _accountService.SetUserStatus(request.UserId, request.status);

                if (result.Success)
                {
                    return Content(HttpStatusCode.OK, "User status updated to " + request.status);
                }
                else
                {
                    switch (result.ResponseError)
                    {
                        case ResponseErrors.EntityNotFound:
                            return Content(HttpStatusCode.NotFound, "A user with the specified ID " + request.UserId + " was not found.");
                        case ResponseErrors.NullParameter:
                            return Content(HttpStatusCode.BadRequest, "One of the specified parameters was null or invalid.");
                        default:
                            return BadRequest();
                    }
                }
            }
        }
    }
}
