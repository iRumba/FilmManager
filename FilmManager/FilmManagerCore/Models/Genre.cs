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
        long _genreId;
        string _name;

        public long GenreId
        {
            get
            {
                return _genreId;
            }

            set
            {
                if (_genreId != value)
                {
                    _genreId = value;
                    OnChanged();
                }
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnChanged();
                }
            }
        }

        public Genre(DbModels.Genre source) : base(source) { }
        public Genre() : base(new DbModels.Genre()) { }

        protected internal override void FillFromModel()
        {
            BeginEdit();
            GenreId = _source.GenreId;
            Name = _source.Name;
            EndEdit();
        }

        internal override DbModels.Genre FillModel()
        {
            _source.GenreId = GenreId;
            _source.Name = Name;
            return _source;
        }
    }
}
