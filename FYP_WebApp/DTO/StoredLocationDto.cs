using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace FYP_WebApp.DTO
{
    [DataContract(Name = "Location")]
    public class StoredLocationDto
    {
        [DataMember(Name = "Location_ID")]
        public int Id { get; set; }
        [DataMember(Name= "Location_Label")]
        public string Label { get; set; }
        [DataMember(Name = "Location_Latitude")]
        public decimal Latitude { get; set; }
        [DataMember(Name = "Location_Longitude")]
        public decimal Longitude { get; set; }
        [DataMember(Name = "Location_Notes")]
        public List<NoteDto> Notes { get; set; }
    }
}