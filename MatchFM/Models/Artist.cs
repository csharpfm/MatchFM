using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    public class Artist
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [StringLength(36)]
        [Index(name: "ArtistMbID", IsUnique = true)]
        public string MbId { get; set; }

        [DataMember]
        public string Image { get; set; }

        public ICollection<Album> Albums { get; set; }

        [DataMember]
        public ICollection<Tag> Tags { get; set; }
    }
}
