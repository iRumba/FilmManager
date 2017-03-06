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
            //_filmDataAdapter.AddFilm
            //    (
            //    new FilmDataLayer.Models.Film
            //    {
            //        LocalName = "qwe",
            //        OriginalName = "asd",
            //        Genres = new List<FilmDataLayer.Models.Genre>
            //        {
            //            new FilmDataLayer.Models.Genre {Name="Horror" }
            //        }
            //    }
            //    );
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

                var results = res.Select(f => DbToAppModelsConverter.ConvertFromDb<FilmDataLayer.Models.Film, Film>(f)).ToList();
                foreach(var film in results)
                    film.SelfRatingChanged += Film_SelfRatingChanged;
                Films = results;

            }
        }

        private async void Film_SelfRatingChanged(object sender, EventArgs e)
        {
            await AddOrUpdateFilmsAsync((Film)sender);
        }

        public async Task RefreshAsync()
        {
            await Task.Run((Action)Refresh);
        }

        //public void AddOrUpdateFilm(Film film)
        //{
        //    _filmDataAdapter.AddOrUpdateFilms(DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(film));
        //}

        //public void AddFilm(Film film)
        //{
        //    _filmDataAdapter.AddOrUpdateFilms(DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(film));
        //}

        //public async Task AddFilmAsync(Film film)
        //{
        //    await Task.Run(() => { AddFilm(film); });
        //}

        public void AddOrUpdateFilms(params Film[] films)
        {
            _filmDataAdapter.AddOrUpdateFilms(films.Select(f => DbToAppModelsConverter.ConvertFromDb<Film, FilmDataLayer.Models.Film>(f)).ToArray());
        }

        //public async Task AddOrUpdateFilmAsync(Film film)
        //{
        //    await Task.Run(() => { AddOrUpdateFilm(film); });
        //}

        public async Task AddOrUpdateFilmsAsync(params Film[] films)
        {
            await Task.Run(() => { AddOrUpdateFilms(films); });
        }
    }
}
