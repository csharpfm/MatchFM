using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    public class Tag
    {
        [Key]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string MbId { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Album> Albums { get; set; }

        public ICollection<Track> Tracks { get; set; }
    }
}
