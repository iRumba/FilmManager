using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using FilmDataLayer.Models;
using DbModels = FilmDataLayer.Models;

namespace FilmManagerCore.Models
{
    public class Film : DbModelReflection<DbModels.Film>
    {
        long _filmId;
        string _originalName;
        string _localName;
        int? _year;
        DateTime _addingDate;
        string _description;
        string _posterUrl;
        int _selfRating;
        float? _globalRating;
        string _foreignUrl;

        //public event EventHandler SelfRatingChanged;

        public long FilmId
        {
            get
            {
                return _filmId;
            }
            set
            {
                if (_filmId != value)
                {
                    _filmId = value;
                    OnChanged();
                }
            }
        }

        public string OriginalName
        {
            get
            {
                return _originalName;
            }
            set
            {
                if (_originalName != value)
                {
                    _originalName = value;
                    OnChanged();
                }
            }
        }

        public string LocalName
        {
            get
            {
                return _localName;
            }
            set
            {
                if (_localName != value)
                {
                    _localName = value;
                    OnChanged();
                }
            }
        }

        public int? Year
        {
            get
            {
                return _year;
            }
            set
            {
                if (_year != value)
                {
                    _year = value;
                    OnChanged();
                }
            }
        }

        public DateTime AddingDate
        {
            get
            {
                return _addingDate;
            }
            set
            {
                if (_addingDate != value)
                {
                    _addingDate = value;
                    OnChanged();
                }
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnChanged();
                }
            }
        }

        public string PosterUrl
        {
            get
            {
                return _posterUrl;
            }
            set
            {
                if (_posterUrl != value)
                {
                    _posterUrl = value;
                    OnChanged();
                }
            }
        }

        public int SelfRating
        {
            get
            {
                return _selfRating;
            }
            set
            {
                if (_selfRating != value)
                {
                    _selfRating = value;
                    OnChanged();
                    //SelfRatingChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public float? GlobalRating
        {
            get
            {
                return _globalRating;
            }
            set
            {
                if (_globalRating != value)
                {
                    _globalRating = value;
                    OnChanged();
                }
            }
        }

        public string ForeignUrl
        {
            get
            {
                return _foreignUrl;
            }
            set
            {
                if (_foreignUrl != value)
                {
                    _foreignUrl = value;
                    OnChanged();
                }
            }
        }

        public List<Genre> Genres { get; set; }

        public Film(DbModels.Film source) : base(source) { }

        public Film() : base(new DbModels.Film()) { }

        //public Film CreateCopy()
        //{
        //    var res = new Film();
        //    res.FillFrom(this);
        //    return res;
        //}

        //public void FillFrom(Film film)
        //{
        //    _addingDate = film._addingDate;
        //    _description = film._description;
        //    _filmId = film._filmId;
        //    _foreignUrl = film._foreignUrl;
        //    _globalRating = film._globalRating;
        //    _localName = film._localName;
        //    _originalName = film._originalName;
        //    _posterUrl = film._posterUrl;
        //    _selfRating = film._selfRating;
        //    _year = film._year;
        //    Genres.Clear();
        //    foreach (var genre in film.Genres)
        //        Genres.Add(genre);
        //    OnPropertyChanged(string.Empty);
        //}

        internal override DbModels.Film FillModel()
        {
            _source.FilmId = FilmId;
            _source.AddingDate = AddingDate;
            _source.Description = Description;
            _source.ForeignUrl = ForeignUrl;
            _source.GlobalRating = GlobalRating;
            _source.LocalName = LocalName;
            _source.OriginalName = OriginalName;
            _source.PosterUrl = PosterUrl;
            _source.SelfRating = SelfRating;
            _source.Year = Year;
            //if (_source.Genres != null)
            //{
            //    var existed = _source.Genres.ToList();
            //    foreach(var genre in existed)
            //    {
            //        var finded = Genres.FirstOrDefault(g => g.GenreId == genre.GenreId || g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase));
            //        if (finded == null)
            //            _source.Genres.Remove(genre);
            //    }
            //    foreach(var genre in Genres)
            //    {
            //        var finded = _source.Genres.FirstOrDefault(g => g.GenreId == genre.GenreId || g.Name.Equals(genre.Name, StringComparison.CurrentCultureIgnoreCase));
            //        if (finded == null)
            //            _source.Genres.Add(genre.FillModel());
            //    }
            //}
            //else
                _source.Genres = Genres?.Select(g => g.FillModel()).ToList() ?? new List<DbModels.Genre>();
            return _source;
        }



        protected internal override void FillFromModel()
        {
            BeginEdit();
            FilmId = _source.FilmId;
            AddingDate = _source.AddingDate;
            Description = _source.Description;
            ForeignUrl = _source.ForeignUrl;
            GlobalRating = _source.GlobalRating;
            LocalName = _source.LocalName;
            OriginalName = _source.OriginalName;
            PosterUrl = _source.PosterUrl;
            SelfRating = _source.SelfRating;
            Year = _source.Year;
            Genres = _source.Genres?.Select(g => new Genre(g)).ToList();
            EndEdit();
        }
    }
}
