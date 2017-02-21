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
    [DataContract]
    public class Film : Notifier
    {
        int _filmId;
        string _originalName;
        string _localName;
        int? _year;
        DateTime _addingDate;
        string _description;
        string _posterUrl;
        int _selfRating;
        float? _globalRating;
        string _foreignUrl;

        [DataMember]
        public int FilmId
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

        [DataMember]
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

        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
        [DataMember]
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
                }
            }
        }
        [DataMember]
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
        [DataMember]
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

        [DataMember]
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
            AddingDate = film.AddingDate;
            Description = film.Description;
            FilmId = film.FilmId;
            ForeignUrl = film.ForeignUrl;
            Genres = film.Genres;
            GlobalRating = film.GlobalRating;
            LocalName = film.LocalName;
            OriginalName = film.OriginalName;
            PosterUrl = film.PosterUrl;
            SelfRating = film.SelfRating;
            Year = film.Year;
        }
    }
}
