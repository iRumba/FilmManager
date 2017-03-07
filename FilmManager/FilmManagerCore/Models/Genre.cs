using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using DbModels = FilmDataLayer.Models;

namespace FilmManagerCore.Models
{
    public class Genre : DbModelReflection<DbModels.Genre>
    {
        public long GenreId { get; set; }
        public string Name { get; set; }
        //public List<Film> Films { get; set; }

        public Genre(DbModels.Genre source) : base(source) { }
        public Genre() : base(new DbModels.Genre()) { }

        internal override void FillFromModel()
        {
            //Films = new List<Film>();
            GenreId = _source.GenreId;
            Name = _source.Name;
            //Films = _source.Films?.Select(f => new Film(f)).ToList();
            //Films.AddRange(_source.Films.Select(f => new Film(f)));
        }

        protected internal override void FillModel()
        {
            _source.GenreId = GenreId;
            _source.Name = Name;
            //_source.Films = Films?.Select(f => f.GetDbModel()).ToList();
            return _source;
        }
    }
}
