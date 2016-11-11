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
    public class Track : Meta
    {
        [DataMember]
        public int Duration { get; set; }

        [Required]
        [DataMember]
        [Index("IX_Name", 2, IsUnique = true)]
        public int AlbumId { get; set; }

        [DataMember]
        public virtual Album Album { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }

        public virtual ICollection<UserTracks> UserTracks { get; set; }
    }
}
