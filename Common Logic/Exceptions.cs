﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FYP_WebApp.Models;

namespace FYP_WebApp.Common_Logic
{
    public class StoredLocationNotFoundException : Exception
    {
        public StoredLocationNotFoundException(string message) : base(message) { }
    }

    public class TeamNotFoundException : Exception
    {
        public TeamNotFoundException(string message) : base(message) { }
    }

    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string message) : base(message) { }
    }

    public class PairingNotFoundException : Exception
    {
        public PairingNotFoundException(string message) : base(message) { }
    }



}