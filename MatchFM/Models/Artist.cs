using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    [DataContract]
    public class Artist : Meta
    {
        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
