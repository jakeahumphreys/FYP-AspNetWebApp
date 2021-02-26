using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Common_Logic
{
    public enum ResponseErrors
    {
        NullParameter = 1,
        NotInserted = 2,
        NotUpdated = 3,
        NotDeleted = 4,
        EntityNotFound = 5
    }
}