using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer.Contexts;
using FilmDataLayer.Models;
using System.Data.Entity.Migrations;
using System.Data.Entity;
using RefactorThis.GraphDiff;

namespace FilmDataLayer
{
    public class FilmDataAdapter
    {
        string _connectionString;
        public FilmDataAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        public QuerySelectBuilder<Film> GetFilmsQueryBulder()
        {
            var context = CreateFilmContext();
            var res = new QuerySelectBuilder<Film>(context.Films);
            res.Disposing += (o, e) => context.Dispose();
            return res;
        }

        public QuerySelectBuilder<Genre> GetGenresQueryBulder()
        {
            var context = CreateFilmContext();
            var res = new QuerySelectBuilder<Genre>(context.Genres);
            res.Disposing += (o, e) => context.Dispose();
            return res;
        }

        public List<int> GetFilmYears()
        {
            using (var context = CreateFilmContext())
            {
                var res = new List<int>();
                res.AddRange(context.Films.Where(f => f.Year.HasValue).GroupBy(f => f.Year.Value).Select(g => g.Key));
                return res;
            }
        }

        public List<Genre> GetFilmGenres(bool usedOnly)
        {
            using (var context = CreateFilmContext())
            {
                var res = new List<Genre>();
                IQueryable<Genre> query = context.Genres;
                if (usedOnly)
                    query = query.Where(g => g.Films.Count > 0);
                res.AddRange(query);
                return res;
            }
        }

        public void AddOrUpdateFilms(params Film[] films)
        {
            using (var context = CreateFilmContext())
            {
                NormalizeContextForUpdate(context, films);
                context.SaveChanges();
            }
        }

        public Film CreateNewFilm()
        {
            using (var context = CreateFilmContext())
            {
                return context.Films.Create();
            }
        }

        public Genre CreateNewGenre()
        {
            using (var context = CreateFilmContext())
            {
                return context.Genres.Create();
            }
        }

        public void RemoveFilms(params Film[] films)
        {
            using (var context = CreateFilmContext())
            {
                
                foreach (var film in films)
                {
                    context.Films.Attach(film);
                    context.Films.Remove(film);
                } 
                context.SaveChanges();
            }
        }

        FilmsContext CreateFilmContext()
        {
            var res = new FilmsContext(_connectionString);
            return res;
        }

        void NormalizeContextForUpdate(FilmsContext context, params Film[] films)
        {
            try
            {
                context.Configuration.AutoDetectChangesEnabled = false;
                foreach (var film in films)
                {
                    var newGenres = new List<Genre>();
                    newGenres.AddRange(film.Genres);
                    film.Genres.Clear();
                    context.Films.Attach(film);
                    if (film.FilmId != default(long) && context.Films.Any(f => f.FilmId == film.FilmId))
                    {
                        context.Entry(film).Collection(f => f.Genres).Load();
                        context.Entry(film).State = EntityState.Modified;
                    }
                    else
                    {
                        context.Entry(film).State = EntityState.Added;
                    }
                        
                    foreach(var genre in newGenres)
                    {
                        if (!film.Genres.Any(g => g.GenreId == genre.GenreId || g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase)))
                        {
                            context.Genres.Attach(genre);
                            film.Genres.Add(genre);
                            if (!context.Genres.Any(g => g.Name.ToUpper() == genre.Name.ToUpper()))
                                context.Entry(genre).State = EntityState.Added;
                        }
                    }

                    var counter = 0;

                    while (counter < film.Genres.Count)
                    {
                        var genre = film.Genres[counter];
                        if (!newGenres.Any(g => g.GenreId == genre.GenreId || g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase)))
                            film.Genres.Remove(genre);
                        else
                            counter++;
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                context.ChangeTracker.DetectChanges();
                context.Configuration.AutoDetectChangesEnabled = true;
            }

        }
    }
}
