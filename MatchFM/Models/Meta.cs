using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace MatchFM.Models
{
    /// <summary>
    /// Abstract class use to return Artist, Album, Tracks
    /// </summary>
    [DataContract]
    public abstract class Meta
    {
        [Key]
        [DataMember]
        public int Id { get; set; }

        [Required]
        [DataMember]
        [MaxLength(200)]
        [Index("IX_Name", 1, IsUnique = true)]
        public string Name { get; set; }

        [DataMember]
        [StringLength(36)]
        [Index]
        public string MbId { get; set; }
    }
}