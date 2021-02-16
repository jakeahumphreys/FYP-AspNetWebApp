using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Common_Logic
{
    public enum Status
    {
        NotOnShift = 0,
        OnShiftNotMobile = 1,
        TransitToLocation = 2,
        AtLocation = 3,
        Break = 4,
        Unknown = 5,
        AssistanceRequired = 6
    }
}