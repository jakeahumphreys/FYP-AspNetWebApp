using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FYP_WebApp.Models;

namespace FYP_WebApp_Unit_Tests.Helpers
{
    class MockUserDbSet : MockDbSet<ApplicationUser>
    {
        public override ApplicationUser Find(params object[] keyValues)
        {
            return new ApplicationUser {Id = "1"};
        }
    }
}
