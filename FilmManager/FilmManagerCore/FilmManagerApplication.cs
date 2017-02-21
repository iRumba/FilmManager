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

        public int ItemsPerPage { get; set; }
        public int PageCount { get; set; }
        public int CurrentPage { get; set; }
        public int TotalCount { get; set; }

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

                query.AddOrder(f => f.FilmId);

                query.SetPaginate(ItemsPerPage, CurrentPage);

                TotalCount = query.TotalCount;
                PageCount = query.TotalPages.Value;
                CurrentPage = query.CurrentPage.Value;
                 
                Films = query.GetResult().ToList().Select(f => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Film, Film>(f)).ToList();
            }
        }

        //public void RefreshAdditionalData()
        //{
        //    UsedGenres = _filmDataAdapter.GetFilmGenres(true).Select(g => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Genre, Genre>(g)).ToList();
        //}

        //public async Task RefreshAdditionalDataAsync()
        //{
        //    await Task.Run((Action)Refresh);
        //}

        public async Task RefreshAsync()
        {
            await Task.Run((Action)Refresh);
        }

        public void AddOrUpdateFilm(Film film)
        {
            _filmDataAdapter.AddFilm(DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(film));
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
