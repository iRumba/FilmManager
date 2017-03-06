using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.Models
{
    [DataContract]
    public class Film
    {
        [DataMember]
        public long FilmId { get; set; }
        [DataMember]
        public string OriginalName { get; set; }
        [DataMember]
        public string LocalName { get; set; }
        [DataMember]
        public int? Year { get; set; }
        [DataMember]
        public DateTime AddingDate { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string PosterUrl { get; set; }
        [DataMember]
        public int SelfRating { get; set; }
        [DataMember]
        public float? GlobalRating { get; set; }
        [DataMember]
        public string ForeignUrl { get; set; }
        [DataMember]
        public List<Genre> Genres { get; set; }
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

            List<Genre> findedAndAddedGenres = new List<Genre>();
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
