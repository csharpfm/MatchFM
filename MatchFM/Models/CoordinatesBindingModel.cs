using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MatchFM.Models
{
    [DataContract]
    public class CoordinatesBindingModel
    {
        [DataMember]
        public double latitude { get; set; }

        [DataMember]
        public double longitude { get; set; }
    }
}