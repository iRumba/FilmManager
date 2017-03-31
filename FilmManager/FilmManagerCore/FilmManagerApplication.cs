﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer;
using FilmManagerCore.Filters;
using FilmManagerCore.Models;
using FilmManagerCore.Parsing;

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
            Logger = new Logger();
            Parsers = new ParsersCollection();
            //Parsers.Load("parsersConfig.xml");
            //Parsers.First().FilmListParsed += (o, e) =>
            //{
            //    foreach (var url in e.FilmUrls)
            //        Console.WriteLine(url);
            //};
            //Parsers.First().StartFilmGettingAsync().ConfigureAwait(false);
            var parser = new Parser();
            parser.Name = "Кинопоиск";
            parser.BaseUrl = "https://www.kinopoisk.ru/";
            parser.FilmLists.Add(new FilmListInfo
            {
                Name = "Топ 250",
                Url = "%host%/top/",
                Films = new HtmlValueGetterInfo { }
            });

            Parsers.Add(parser);
            Parsers.Save("parsersConfig.xml");
        }

        public ParsersCollection Parsers { get; }

        public FilmFilters Filters { get; set; }

        public List<Film> Films { get; set; }

        public List<Genre> UsedGenres
        {
            get
            {
                return _filmDataAdapter.GetFilmGenres(true).Select(g => new Genre(g)).ToList();
            }
        }

        public List<Genre> AllGenres
        {
            get
            {
                return _filmDataAdapter.GetFilmGenres(false).Select(g => new Genre(g)).ToList();
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

        public Logger Logger { get; }

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
                if (!string.IsNullOrWhiteSpace(Filters.TextFilter))
                    query.AddFilter(f => f.LocalName.ToUpper().Contains(Filters.TextFilter.ToUpper()) ||
                    f.OriginalName.ToUpper().Contains(Filters.TextFilter.ToUpper()) ||
                    f.Description.ToUpper().Contains(Filters.TextFilter.ToUpper()));
                if (Filters.Year.HasValue)
                    query.AddFilter(f => f.Year == Filters.Year.Value);

                query.AddOrder(f => f.FilmId);

                TotalCount = query.TotalCount;

                List<Infrastructure.Models.Film> res = null;
                if (ItemsPerPage > 0 && CurrentPage > 0)
                {
                    if (!query.IsOrdered)
                        query.AddOrder(f => f.FilmId);
                    PageCount = (int)Math.Ceiling((decimal)TotalCount / ItemsPerPage);
                     
                    res = query.GetPage(ItemsPerPage, CurrentPage).ToList();
                }
                    
                else
                    res = query.GetResult().ToList();

                var results = res.Select(f => new Film(f)).ToList();
                foreach(var film in results)
                    film.Changed += Film_Changed;

                Films = results;
            }
        }

        void Film_Changed(object sender, EventArgs e)
        {
            AddOrUpdateFilms((Film)sender);
        }

        async void Film_SelfRatingChanged(object sender, EventArgs e)
        {
            await AddOrUpdateFilmsAsync((Film)sender);
        }

        public async Task RefreshAsync()
        {
            await Task.Run((Action)Refresh);
        }

        public void AddOrUpdateFilms(params Film[] films)
        {
            _filmDataAdapter.AddOrUpdateFilms(films.Select(f => f.FillModel()).ToArray());
            Logger.AddMessage(new LogMessage { Message = "Фильмы отредактированы", Type = "Info" });
        }

        public async Task AddOrUpdateFilmsAsync(params Film[] films)
        {
            await Task.Run(() => { AddOrUpdateFilms(films); });
        }

        public void RemoveFilms(params Film[] films)
        {
            _filmDataAdapter.RemoveFilms(films.Select(f => f.Source).ToArray());
        }

        public async Task RemoveFilmsAsync(params Film[] films)
        {
            await Task.Run(() => { RemoveFilms(films); });
        }
    }
}
