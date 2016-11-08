using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    [DataContract]
    public class UserTracks
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [DataMember]
        [Required]
        public int TrackId { get; set; }

        [DataMember]
        public virtual Track Track { get; set; }

        [DataMember]
        [Required]
        public DateTime ListenDate { get; set; }
    }
}
