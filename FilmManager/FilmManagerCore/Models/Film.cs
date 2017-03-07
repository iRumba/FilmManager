using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FilmManagerCore.Models
{
    public class Film : Notifier
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

        public event EventHandler SelfRatingChanged;
        
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
                    OnPropertyChanged(nameof(FilmId));
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
                if(_originalName!= value)
                {
                    _originalName = value;
                    OnPropertyChanged(nameof(OriginalName));
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
                    OnPropertyChanged(nameof(LocalName));
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
                    OnPropertyChanged(nameof(Year));
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
                    OnPropertyChanged(nameof(AddingDate));
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
                    OnPropertyChanged(nameof(Description));
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
                    OnPropertyChanged(nameof(PosterUrl));
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
                    OnPropertyChanged(nameof(SelfRating));
                    SelfRatingChanged?.Invoke(this, new EventArgs());
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
                    OnPropertyChanged(nameof(GlobalRating));
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
                    OnPropertyChanged(nameof(ForeignUrl));
                }
            }
        }
        
        public ObservableCollection<Genre> Genres { get; internal set; }

        public Film()
        {
            Genres = new ObservableCollection<Genre>();
        }

        public Film CreateCopy()
        {
            var res = new Film();
            res.FillFrom(this);
            return res;
        }

        public void FillFrom(Film film)
        {
            _addingDate = film._addingDate;
            _description = film._description;
            _filmId = film._filmId;
            _foreignUrl = film._foreignUrl;
            _globalRating = film._globalRating;
            _localName = film._localName;
            _originalName = film._originalName;
            _posterUrl = film._posterUrl;
            _selfRating = film._selfRating;
            _year = film._year;
            Genres.Clear();
            foreach (var genre in film.Genres)
                Genres.Add(genre);
            OnPropertyChanged(string.Empty);
        }
    }
}
