using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MatchFM.Models
{
    [DataContract]
    public class Matches
    {
        [DataMember]
        [Key]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        [DataMember]
        [Required]
        public string ProfilId { get; set; }
        public virtual ApplicationUser Profil { get; set; }

        [DataMember]
        [Required]
        public bool Match { get; set; }
    }
}