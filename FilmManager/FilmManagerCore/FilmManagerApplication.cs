using System;
using System.Collections.Generic;
using System.Globalization;
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
            PageCount = 1;
            ItemsPerPage = 25;
        }

        public FilmFilters Filters { get; set; }

        public List<Film> Films { get; set; }

        public List<Genre> UsedGenres
        {
            get
            {
                return _filmDataAdapter.GetFilmGenres(true).Select(g => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Genre, Genre>(g)).ToList();
            }
        }

        public List<Genre> AllGenres
        {
            get
            {
                return _filmDataAdapter.GetFilmGenres(false).Select(g => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Genre, Genre>(g)).ToList();
            }
        }

        public List<int> Years
        {
            get
            {
                return _filmDataAdapter.GetFilmYears();
            }
        }

        public int ItemsPerPage { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }

        public void Refresh()
        {
            using (var query = _filmDataAdapter.GetFilmsQueryBulder())
            {
                byte[] bytes = null;
                var str = string.Empty;
                if (!string.IsNullOrWhiteSpace(Filters.TextFilter))
                {
                    bytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(Filters.TextFilter.ToUpper()));
                    str = Encoding.UTF8.GetString(bytes);
                }


                var stringComparer = EqualityComparer<string>.Default;
                if (Filters.GenreId.HasValue)
                    query.AddFilter(f => f.Genres.Any(g => g.GenreId == Filters.GenreId.Value));
                if (Filters.Rating.HasValue)
                    query.AddFilter(f => f.GlobalRating >= Filters.Rating.Value);
                if (Filters.SelfRating.HasValue)
                    query.AddFilter(f => f.SelfRating == Filters.SelfRating.Value);
                //if (!string.IsNullOrWhiteSpace(Filters.TextFilter))
                //    query.AddFilter(f => f.LocalName.ToLower().Contains(Filters.TextFilter.ToLower()) ||
                //    f.OriginalName.ToLower().Contains(Filters.TextFilter.ToLower()) ||
                //    f.Description.ToLower().Contains(Filters.TextFilter.ToLower()));
                if (!string.IsNullOrWhiteSpace(Filters.TextFilter))
                    query.AddFilter(f => f.LocalName.ToUpper().Contains(str) ||
                    f.OriginalName.ToUpper().Contains(str) ||
                    f.Description.ToUpper().Contains(str));
                //if (!string.IsNullOrWhiteSpace(Filters.TextFilter))
                //    query.AddFilter(f => f.LocalName.IndexOf(str,StringComparison.CurrentCultureIgnoreCase) >= 0 ||
                //    f.OriginalName.ToUpper().Contains(str) ||
                //    f.Description.ToUpper().Contains(str));
                if (Filters.Year.HasValue)
                    query.AddFilter(f => f.Year == Filters.Year.Value);

                query.AddOrder(f => f.FilmId);

                TotalCount = query.TotalCount;
                //CurrentPage = query.CurrentPage.Value;

                List<FilmDataLayer.Models.Film> res = null;
                if (ItemsPerPage > 0 && CurrentPage > 0)
                {
                    if (!query.IsOrdered)
                        query.AddOrder(f => f.FilmId);
                    PageCount = (int)Math.Ceiling((decimal)TotalCount / ItemsPerPage);
                     
                    res = query.GetPage(ItemsPerPage, CurrentPage).ToList();
                }
                    
                else
                    res = query.GetResult().ToList();
                    //query.SetPaginate(ItemsPerPage, CurrentPage);
                 
                Films = res.Select(f => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Film, Film>(f)).ToList();
            }
        }

        public async Task RefreshAsync()
        {
            await Task.Run((Action)Refresh);
        }

        public void AddOrUpdateFilm(Film film)
        {
            _filmDataAdapter.AddOrUpdateFilm(DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(film));
        }

        public void AddFilm(Film film)
        {
            _filmDataAdapter.AddFilm(DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(film));
        }

        public async Task AddFilmAsync(Film film)
        {
            await Task.Run(() => { AddFilm(film); });
        }

        public void AddOrUpdateFilms(IEnumerable<Film> films)
        {
            _filmDataAdapter.AddFilms(films.Select(f => DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(f)));
        }

        public async Task AddOrUpdateFilmAsync(Film film)
        {
            await Task.Run(() => { AddOrUpdateFilm(film); });
        }

        public async Task AddOrUpdateFilmsAsync(IEnumerable<Film> films)
        {
            await Task.Run(() => { AddOrUpdateFilms(films); });
        }
    }
}
