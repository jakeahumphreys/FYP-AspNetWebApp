using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services.Description;
using FYP_WebApp.Common_Logic;
using FYP_WebApp.DataAccessLayer;
using FYP_WebApp.Models;
using Microsoft.Owin.Security.Facebook;
using WebGrease.Css;

namespace FYP_WebApp.ServiceLayer
{
    public class NoteService
    {
        private readonly INoteRepository _noteRepository;

        public NoteService()
        {
            _noteRepository = new NoteRepository(new ApplicationDbContext());
        }

        public NoteService(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public List<Note> GetAll()
        {
            return _noteRepository.GetAll();
        }

        public Note GetDetails(int id)
        {
            if (id == 0)
            {
                throw new ArgumentException("No ID specified");
            }

            var note = _noteRepository.GetById(id);

            if (note == null)
            {
                throw new NoteNotFoundException("A Note with ID " + id + " was not found.");
            }

            return note;
        }

        public ServiceResponse Create(Note note)
        {
            if (note == null)
            {
                return new ServiceResponse {Success = false, ResponseError = ResponseErrors.NullParameter};
            }
            else
            {
                if (note.SenderId == null)
                {
                    return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
                }

                if (note.TimeCreated == DateTime.MinValue)
                {
                    return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter};

                }
                _noteRepository.Insert(note);
                _noteRepository.Save();
                return new ServiceResponse {Success = true};
            }
        }

        public ServiceResponse Edit(Note note)
        {
            if (note == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                var existingNote = _noteRepository.GetById(note.Id);
                existingNote.Content = note.Content;
                existingNote.IsInactive = note.IsInactive;
                existingNote.StoredLocationId = note.StoredLocationId;

                _noteRepository.Update(existingNote);
                _noteRepository.Save();

                return new ServiceResponse { Success = true };
            }
        }

        public ServiceResponse Delete(Note note)
        {
            if (note == null)
            {
                return new ServiceResponse { Success = false, ResponseError = ResponseErrors.NullParameter };
            }
            else
            {
                _noteRepository.Delete(note);
                _noteRepository.Save();
                return new ServiceResponse { Success = true };
            }
        }

        public void Dispose()
        {
            _noteRepository.Dispose();
        }
    }
}