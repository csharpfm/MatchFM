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
    public class Album : Meta
    {
        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [DataMember]
        public int ArtistId { get; set; }

        [DataMember]
        public virtual Artist Artist { get; set; }

        [DataMember]
        public virtual ICollection<Track> Tracks { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }
}
