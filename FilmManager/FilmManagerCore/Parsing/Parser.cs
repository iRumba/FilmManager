using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Models;

namespace FilmManagerCore.Parsing
{
    public class Parser
    {
        public event EventHandler<FilmsGettingEventArgs> FilmListParsed;
        public event EventHandler<FilmsGettingErrorEventArgs> FilmListParsingError;

        public Parser()
        {
            Film = new FilmInfo();
            FilmLists = new List<FilmListInfo>();
        }

        public string Name { get; set; }

        public string BaseUrl { get; set; }

        public FilmInfo Film { get; }

        public List<FilmListInfo> FilmLists { get; }

        public Film ParseFilm(string url)
        {
            var document = HtmlValueGetter.CreateDocument(url);
            var description = Film.Description?.GetValue(document);
            float? globalRating = null;
            float f;
            if (float.TryParse(Film.GlobalRating?.GetValue(document) ?? string.Empty, out f))
                globalRating = f;
            var localName = Film.LocalName?.GetValue(document);
            var originalName = Film.OriginalName?.GetValue(document);
            var posterUrl = Film.PosterUrl?.GetValue(document);
            int? year = null;
            int i;
            if (int.TryParse(Film.Year?.GetValue(document) ?? string.Empty, out i))
                year = i;
            var genres = new List<Genre>();
            var parsedGenres = Film.Genres?.GetValues(document).Select(v => new Genre { Name = v });
            if (parsedGenres != null)
                genres.AddRange(parsedGenres);

            var res = new Film
            {
                Description = description,
                ForeignUrl = url,
                GlobalRating = globalRating,
                LocalName = localName,
                OriginalName = originalName,
                PosterUrl = posterUrl,
                Year = year,
                Genres = genres,
            };
            return res;
        }

        public async Task<Film> ParseFilmAsync(string url)
        {
            return await Task.Run(() => ParseFilm(url));
        }

        public async Task StartFilmGettingAsync()
        {
            foreach(var flInfo in FilmLists)
            {
                string[] urls;
                try
                {
                    urls = await GetFilmsAsync(flInfo);
                    FilmListParsed?.Invoke(this, new FilmsGettingEventArgs { FilmUrls = urls, PageName = flInfo.Name, PageUrl = flInfo.Url });
                }
                catch(Exception ex)
                {
                    FilmListParsingError?.Invoke(this, new FilmsGettingErrorEventArgs { ErrorMessage = ex.Message, PageName = flInfo.Name, PageUrl = flInfo.Url });
                }
            }
        }

        string[] GetFilms(FilmListInfo filmListInfo)
        {
            var envVars = CreateVariablesManager();
            var url = envVars.Replace(filmListInfo.Url);
            var res = filmListInfo.Films?.GetValues(url);
            if (res == null)
                throw new InvalidOperationException($"Невозможно получить список фильмов по ссылке {url}");
            res = res.Select(s =>
            {
                Uri uri;
                if (Uri.TryCreate(s, UriKind.Absolute, out uri) || Uri.TryCreate($"{BaseUrl.TrimEnd('/')}/{s.TrimStart('/')}", UriKind.Absolute, out uri))
                    return uri.ToString();
                //return s;
                throw new InvalidCastException($"Не получается создать Uri из {s}");
            }).ToArray();
            return res;
        }

        async Task<string[]> GetFilmsAsync(FilmListInfo filmListInfo)
        {
            return await Task.Run(() => GetFilms(filmListInfo));
        }

        EnvironmentVariableManager CreateVariablesManager()
        {
            var res = new EnvironmentVariableManager();
            res.SetVariable("host", BaseUrl.TrimEnd('/'));
            return res;
        }
    }

    public class FilmsGettingEventArgs : EventArgs
    {
        public string[] FilmUrls { get; set; }
        public string PageName { get; set; }
        public string PageUrl { get; set; }
    }

    public class FilmsGettingErrorEventArgs : EventArgs
    {
        public string PageName { get; set; }
        public string PageUrl { get; set; }
        public string ErrorMessage { get; set; }

    }
}
