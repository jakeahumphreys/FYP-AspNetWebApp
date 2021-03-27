using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using FYP_WebApp.Common_Logic;

namespace FYP_WebApp.DTO
{
    [DataContract(Name="Status")]
    public class StatusDto
    {
        [DataMember(Name = "Status_User_ID")]
        public string UserId { get; set; }
        [DataMember(Name = "Status_User_Status")]
        public Status status { get; set; }
    }
}