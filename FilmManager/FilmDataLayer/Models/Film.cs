using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.Models
{
    [DataContract]
    public class Film
    {
        [DataMember]
        public long FilmId { get; set; }
        [DataMember]
        public string OriginalName { get; set; }
        [DataMember]
        public string LocalName { get; set; }
        [DataMember]
        public int? Year { get; set; }
        [DataMember]
        public DateTime AddingDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string PosterUrl { get; set; }
        [DataMember]
        public int SelfRating { get; set; }
        [DataMember]
        public float? GlobalRating { get; set; }
        [DataMember]
        public string ForeignUrl { get; set; }
        [DataMember]
        public List<Genre> Genres { get; set; }
        public Film()
        {
            Genres = new List<Genre>();
        }
    }
}
