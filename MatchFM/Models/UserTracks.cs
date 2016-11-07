using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    public class UserTracks
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [DataMember]
        [Required]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        [DataMember]
        [Required]
        public int TrackId { get; set; }

        [DataMember]
        public Track Track { get; set; }

        [Required]
        public DateTime ListenDate { get; set; }
    }
}
