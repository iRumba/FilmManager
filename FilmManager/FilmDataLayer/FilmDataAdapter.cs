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
                //foreach (var film in films)
                //    context.Films.Attach(film);
                NormalizeContextForUpdate(context, films);
                //context.Configuration.ProxyCreationEnabled = false;
                //context.Films.AddOrUpdate(films);
                //context.Configuration.AutoDetectChangesEnabled = false;
                //foreach (var film in films)
                //{
                //    context.Films.AddOrUpdate(film);
                //}
                //context.ChangeTracker.DetectChanges();
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
                    //context.Entry(film).State = EntityState.Deleted;
                }
                    
                context.SaveChanges();
            }
        }

        //public void AddFilms(IEnumerable<Film> films)
        //{
        //    using (var context = CreateFilmContext())
        //    {
        //        context.Films.AddRange(films);
        //        //context.Configuration.AutoDetectChangesEnabled = false;
        //        //foreach (var film in films)
        //        //{
        //        //    context.Films.AddOrUpdate(film);
        //        //}
        //        //context.ChangeTracker.DetectChanges();
        //        context.SaveChanges();
        //    }
        //}

        //public void AddFilm(Film film)
        //{
        //    using (var context = CreateFilmContext())
        //    {
        //        //context.Entry(film).State = EntityState.Added;
        //        NormalizeContextForUpdate(context, film);
        //        context.Films.Add(film);
        //        //foreach (var genre in film.Genres)
        //        //    context.Genres.AddOrUpdate(genre);
        //        //context.ChangeTracker.DetectChanges();
        //        context.SaveChanges();
        //    }
        //}

        //public void AddOrUpdateFilm(Film film)
        //{
        //    using (var context = CreateFilmContext())
        //    {
        //        NormalizeContextForUpdate(context, film);
        //        context.Films.AddOrUpdate(film);
        //        context.SaveChanges();
        //    }
        //}

        FilmsContext CreateFilmContext()
        {
            var res = new FilmsContext(_connectionString);
            return res;
        }

        void NormalizeContextForUpdate(FilmsContext context, params Film[] films)
        {
            try
            {
                //context.Configuration.AutoDetectChangesEnabled = false;
                //var genres = new List<Genre>();
                foreach (var film in films)
                {
                    context.UpdateGraph(film, map => map.OwnedCollection(f => f.Genres));
                    //IEnumerable<Genre> deletedGenres = null;
                    //var findedFilm = film.FilmId != default(long) ? context.Films.Include(f => f.Genres).AsNoTracking().SingleOrDefault(f => f.FilmId == film.FilmId) : null;
                    //if (findedFilm != null)
                    //{
                    //    context.Entry(film).State = EntityState.Modified;

                    //    deletedGenres = findedFilm.Genres.Except(film.Genres,
                    //        new UniversalEqualityComparer<Genre>((g1, g2) => g1.GenreId == g2.GenreId, g => g.GenreId.GetHashCode()));
                    //    //deletedGenres.ToList().ForEach()
                    //}
                    //else
                    //    context.Entry(film).State = EntityState.Added;

                    //foreach (var genre in film.Genres)
                    //{
                    //    Genre findedGenre = null;
                    //    findedGenre = context.Genres.SingleOrDefault(g => g.GenreId == genre.GenreId || g.Name.ToUpper() == genre.Name.ToUpper());
                    //    //if (findedGenre !=null)

                    //}
                    ////context.Films.Attach(film);
                    ////var findedFilm = context.Films.Find(film.FilmId);
                    ////findedFilm.Genres.AddRange(film.Genres);
                    //var genres = new List<Genre>();
                    //genres.AddRange(film.Genres);
                    //while (film.Genres.Count > 0)
                    //    film.Genres.RemoveAt(0);
                    ////film.Genres.Clear();
                    //film.Genres.AddRange(genres.
                    //    Select(g => g.GenreId > 0 ?
                    //        context.Genres.
                    //            Single(g2 => g2.Name.ToUpper() == g.Name.ToUpper()) :
                    //    context.Genres.Add(g)));
                }
            }
            finally
            {
                //context.ChangeTracker.DetectChanges();
                //context.Configuration.AutoDetectChangesEnabled = true;
            }

        }
    }
}
