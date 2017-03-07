using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManagerCore.Serializers
{
    public static class ModelsHelper
    {
        public static FilmDataLayer.Models.Film FilmToDbModel(this Film film)
        {
            var res = new FilmDataLayer.Models.Film
            {
                AddingDate = film.AddingDate,
                Description = film.Description,
                FilmId = film.FilmId,
                ForeignUrl = film.ForeignUrl,
                GlobalRating = film.GlobalRating,
                LocalName = film.LocalName,
                OriginalName = film.OriginalName,
                PosterUrl = film.PosterUrl,
                SelfRating = film.SelfRating,
                Year = film.Year,
            };

            return res;
        }

        public static FilmDataLayer.Models.Genre GenreToDbModel(this Genre genre)
    }
}
