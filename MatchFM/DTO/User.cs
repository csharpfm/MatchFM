using MatchFM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MatchFM.DTO
{
    [DataContract]
    public class User
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Username { get; set; }
        [DataMember]
        public string Photo { get; set; }
        [DataMember]
        public GenderEnum Gender { get; set; }
    }
}