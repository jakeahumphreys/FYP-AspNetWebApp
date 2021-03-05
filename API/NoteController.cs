using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DTO;
using FYP_WebApp.Models;
using FYP_WebApp.ServiceLayer;
using Microsoft.Owin.Security.Facebook;

namespace FYP_WebApp.API
{
    public class NoteController : ApiController
    {
        private readonly NoteService _noteService;
        private readonly StoredLocationService _storedLocationService;
        private readonly Mapper _mapper;

        public NoteController()
        {
            _noteService = new NoteService();
            _storedLocationService = new StoredLocationService();
            var config = AutomapperConfig.instance().Configure();
            _mapper = new Mapper(config);
        }

        [HttpGet]
        public IEnumerable<NoteDto> Get()
        {
            return _mapper.Map<IList<Note>, List<NoteDto>>(_noteService.GetAll());
        }

        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            try
            {
                return Json(_mapper.Map<Note, NoteDto>(_noteService.GetDetails(id)));
            }
            catch (ArgumentException ex)
            {
                return Content(HttpStatusCode.BadRequest, ex.Message);
            }
            catch (NoteNotFoundException ex)
            {
                return Content(HttpStatusCode.NotFound, ex.Message);
            }
        }

        [HttpPost]
        public IHttpActionResult Post(int storedLocationId, [FromBody] NoteDto requestBody)
        {
            if (storedLocationId == 0)
            {
                return Content(HttpStatusCode.BadRequest, "A Location ID is required to add a note.");
;           }

            if (requestBody == null)
            {
                return Content(HttpStatusCode.BadRequest, "No body was sent with the request.");
            }

            if (_storedLocationService.GetDetails(storedLocationId) == null)
            {
                return Content(HttpStatusCode.BadRequest, "A location with the specified ID does not exist.");
            }

            var note = _mapper.Map<NoteDto, Note>(requestBody);
            note.StoredLocationId = storedLocationId;
            var result = _noteService.Create(note);

            if (result.Success)
            {
                return Content(HttpStatusCode.OK, "Note added successfully.");
            }
            else
            {
                switch (result.ResponseError)
                {
                    case ResponseErrors.NullParameter:
                        return Content(HttpStatusCode.BadRequest, "The request body provided was invalid.");
                    default:
                        return BadRequest();
                }

            }
        }


    }
}
