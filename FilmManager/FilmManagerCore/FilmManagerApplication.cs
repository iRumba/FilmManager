using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer;
using FilmManagerCore.Filters;
using FilmManagerCore.Models;
using FilmManagerCore.Serializers;

namespace FilmManagerCore
{
    public class FilmManagerApplication
    {
        FilmDataAdapter _filmDataAdapter;

        public FilmManagerApplication(string connectionString)
        {
            _filmDataAdapter = new FilmDataAdapter(connectionString);
            Filters = new FilmFilters();
            Films = new List<Film>();
        }

        

        public FilmFilters Filters { get; set; }

        public List<Film> Films { get; set; }

        public int ItemsPerPage { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }

        public void Refresh()
        {
            using (var query = _filmDataAdapter.GetFilmsQueryBulder())
            {
                if (Filters.GenreId.HasValue)
                    query.AddFilter(f => f.Genres.Any(g => g.GenreId == Filters.GenreId.Value));
                if (Filters.Rating.HasValue)
                    query.AddFilter(f => f.GlobalRating >= Filters.Rating.Value);
                if (Filters.SelfRating.HasValue)
                    query.AddFilter(f => f.SelfRating == Filters.SelfRating.Value);
                if (!string.IsNullOrWhiteSpace(Filters.TextFilter))
                    query.AddFilter(f => f.LocalName.Contains(Filters.TextFilter) || f.OriginalName.Contains(Filters.TextFilter)
                    || f.Description.Contains(Filters.TextFilter));
                if (Filters.Year.HasValue)
                    query.AddFilter(f => f.Year == Filters.Year.Value);
                
                
                Films = query.GetResult().ToList().Select(f => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Film, Film>(f)).ToList();
            }
        }

        
    }
}
