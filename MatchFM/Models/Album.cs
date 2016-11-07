using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    public class Album
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [StringLength(36)]
        [Index(name: "AlbumMbID", IsUnique = true)]
        public string MbId { get; set; }

        [DataMember]
        public string Image { get; set; }

        [DataMember]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [DataMember]
        public int ArtistId { get; set; }

        public Artist Artist { get; set; }

        public ICollection<Track> Tracks { get; set; }

        [DataMember]
        public ICollection<Tag> Tags { get; set; }
    }
}
