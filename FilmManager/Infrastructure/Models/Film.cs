using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Film
    {
        public long FilmId { get; set; }
        public string OriginalName { get; set; }
        public string LocalName { get; set; }
        public int? Year { get; set; }
        public DateTime AddingDate { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public int SelfRating { get; set; }
        public float? GlobalRating { get; set; }
        public string ForeignUrl { get; set; }
        public virtual List<Genre> Genres { get; set; }
    }
}
