using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.Models
{
    public class Film
    {
        public long FilmId { get; set; }
        public string OriginalName { get; set; }
        public string LocalName { get; set; }
        public int? Year { get; set; }
        public DateTime AddingDate { get; set; }
        public string Description { get; set; }
        public string PosterUrl { get; set; }
        public int SelfRating { get; set; }
        public float? GlobalRating { get; set; }
        public string ForeignUrl { get; set; }
        public virtual List<Genre> Genres { get; set; }
        public Film()
        {
            Genres = new List<Genre>();
        }

        public void CopyFrom(Film film)
        {

            FilmId = film.FilmId;
            AddingDate = film.AddingDate;
            Description = film.Description;
            ForeignUrl = film.ForeignUrl;
            GlobalRating = film.GlobalRating;
            LocalName = film.LocalName;
            OriginalName = film.OriginalName;
            PosterUrl = film.PosterUrl;
            SelfRating = film.SelfRating;
            Year = film.Year;

            var findedAndAddedGenres = new List<Genre>();
            foreach (var genre in film.Genres)
            {
                Genre genreById = null;
                if ((genreById = Genres.FirstOrDefault(g=>g.GenreId == genre.GenreId)) != null)
                {
                    if (genreById.Name != genre.Name)
                        genreById.Name = genre.Name;
                    findedAndAddedGenres.Add(genreById);
                }
                else
                {
                    Genres.Add(genre);
                    findedAndAddedGenres.Add(genre);
                }
            }

            Genres.RemoveAll(r => !findedAndAddedGenres.Contains(r));
        }
    }
}
