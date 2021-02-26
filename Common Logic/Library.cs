using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FYP_WebApp.Common_Logic
{
    public class Library
    {
        public DateTime ConstructDateTime(DateTime Date, DateTime Time)
        {
            return new DateTime(Date.Year, Date.Month, Date.Day, Time.Hour, Time.Minute, Time.Second);
        }

        public List<DateTime> DeconstructDateTime(DateTime Date)
        {
            var deconstructedDate = new List<DateTime>();

            deconstructedDate.Add(new DateTime(Date.Year, Date.Month, Date.Day));
            deconstructedDate.Add(new DateTime(0001,01,01,Date.Hour, Date.Minute, Date.Second));

            return deconstructedDate;
        }
    }
}