using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    public class Track
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [StringLength(36)]
        [Index(name: "TrackMbID", IsUnique = true)]
        public string MbId { get; set; }

        [DataMember]
        public int Duration { get; set; }

        [Required]
        [DataMember]
        public int AlbumId { get; set; }

        public Album Album { get; set; }

        [DataMember]
        public ICollection<Tag> Tags { get; set; }

        public ICollection<UserTracks> UserTracks { get; set; }
    }
}
