using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MatchFM.Models
{
    [DataContract]
    public abstract class Meta
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        [StringLength(36)]
        [Index(IsUnique = true)]
        public string MbId { get; set; }
    }
}