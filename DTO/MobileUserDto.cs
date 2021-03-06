using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FYP_WebApp.DTO
{
    [DataContract( Name = "MobileUser")]
    public class MobileUserDto
    {
        [DataMember(Name = "User_ID")]
        public string Id { get; set; }
        [DataMember(Name = "User_First_Name")]
        public string FirstName { get; set;}
        [DataMember(Name = "User_Surname")]
        public string Surname { get; set; }
    }
}