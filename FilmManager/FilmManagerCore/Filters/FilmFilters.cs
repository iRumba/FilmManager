using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Filters
{
    public class FilmFilters
    {
        public long? GenreId { get; set; }
        public int? Year { get; set; }
        public float? Rating { get; set; }
        public int? SelfRating { get; set; }
        public string TextFilter { get; set; }
    }
}
