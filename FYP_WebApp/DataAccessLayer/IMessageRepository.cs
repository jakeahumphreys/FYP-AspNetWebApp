using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.DataAccessLayer
{
    public interface IMessageRepository : IDisposable
    {
        List<Message> GetAll();
        Message GetById(int id);
        void Insert(Message message);
        void Update(Message message);
        void Delete(Message message);
        void Save();
    }
}