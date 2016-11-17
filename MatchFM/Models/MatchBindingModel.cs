using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MatchFM.Models
{
    /// <summary>
    /// Class use to update match with user
    /// </summary>
    [DataContract]
    public class MatchBindingModel
    {
        [DataMember]
        public string ProfilId { get; set; }

        [DataMember]
        public bool Match { get; set; }
    }
}