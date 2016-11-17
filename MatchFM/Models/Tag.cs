using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace MatchFM.Models
{
    /// <summary>
    /// Class define Tag
    /// </summary>
    [DataContract]
    public class Tag
    {
        [Key]
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string MbId { get; set; }

        public virtual ICollection<Artist> Artists { get; set; }

        public virtual ICollection<Album> Albums { get; set; }

        public virtual ICollection<Track> Tracks { get; set; }
    }
}
