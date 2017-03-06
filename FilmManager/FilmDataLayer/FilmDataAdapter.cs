using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer.Contexts;
using FilmDataLayer.Models;
using System.Data.Entity.Migrations;
using System.Data.Entity;

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
            var res = new QuerySelectBuilder<Film>(context.Films.Include(f => f.Genres));
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

        public void AddOrUpdateFilms(IEnumerable<Film> films)
        {
            using (var context = CreateFilmContext())
            {
                context.Films.AddOrUpdate(films.ToArray());
                //context.Configuration.AutoDetectChangesEnabled = false;
                //foreach (var film in films)
                //{
                //    context.Films.AddOrUpdate(film);
                //}
                //context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            }
        }

        public void AddFilms(IEnumerable<Film> films)
        {
            using (var context = CreateFilmContext())
            {
                context.Films.AddRange(films);
                //context.Configuration.AutoDetectChangesEnabled = false;
                //foreach (var film in films)
                //{
                //    context.Films.AddOrUpdate(film);
                //}
                //context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            }
        }

        public void AddFilm(Film film)
        {
            using (var context = CreateFilmContext())
            {
                //context.Entry(film).State = EntityState.Added;
                NormalizeContextForUpdate(context, film);
                context.Films.Add(film);
                //foreach (var genre in film.Genres)
                //    context.Genres.AddOrUpdate(genre);
                //context.ChangeTracker.DetectChanges();
                context.SaveChanges();
            }
        }

        public void AddOrUpdateFilm(Film film)
        {
            using (var context = CreateFilmContext())
            {
                NormalizeContextForUpdate(context, film);
                context.Films.AddOrUpdate(film);
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
            //var genreNames = new List<string>();
            var genres = new List<Genre>();
            foreach(var film in films)
            {
                //context.Films.Where(f => f.FilmId == film.FilmId).Include(f => f.Genres).Load();
                genres.AddRange(film.Genres);
                //genreNames.AddRange(film.Genres.Select(g=>g.Name));
            }
            var genreNames = genres.Select(g => g.Name);
            var distGenres = genreNames.Distinct(StringComparer.CurrentCultureIgnoreCase);
            var selGenres = context.Genres.Where(g => genreNames.Contains(g.Name)).ToList();
            var addedGenres = distGenres.Except(selGenres.Select(g => g.Name));
            foreach (var genre in genres)
                if (addedGenres.Contains(genre.Name, StringComparer.CurrentCultureIgnoreCase))
                    context.Entry(genre).State = EntityState.Added;
        }
    }
}
