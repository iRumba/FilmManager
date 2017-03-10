using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class FilmVm : ModelReflection<Film>
    {
        string _originalName;
        string _localName;
        int? _year;
        string _description;
        string _posterUrl;
        int _selfRating;
        float? _globalRating;
        string _foreignUrl;

        public bool AutosaveSelfRating { get; set; }

        public string OriginalName
        {
            get
            {
                return _originalName;
            }

            set
            {
                if (OriginalName != value)
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
                if (LocalName != value)
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
                if (Year != value)
                {
                    _year = value;
                    OnPropertyChanged(nameof(Year));
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
                if (Description != value)
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
                if (PosterUrl != value)
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
                if (SelfRating != value)
                {
                    _selfRating = value;
                    OnPropertyChanged(nameof(SelfRating));
                    if (AutosaveSelfRating)
                        Source.SelfRating = value;                        
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
                if (GlobalRating != value)
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
                if (ForeignUrl != value)
                {
                    _foreignUrl = value;
                    OnPropertyChanged(nameof(ForeignUrl));
                }
            }
        }

        public ObservableCollection<GenreVm> Genres { get; set; }

        //public bool Changed
        //{
        //    get
        //    {
        //        return _changed;
        //    }

        //    set
        //    {
        //        if (Changed != value)
        //        {
        //            _changed = value;
        //            OnPropertyChanged(nameof(Changed));
        //        }
        //    }
        //}

        public FilmVm() : this(new Film()) { }
        public FilmVm(Film source) : base(source)
        {
            //FillFromModel();
        }

        protected internal override Film FillModel()
        {
            _source.BeginEdit();
            _source.OriginalName = OriginalName;
            _source.LocalName = LocalName;
            _source.Year = Year;
            _source.SelfRating = SelfRating;
            _source.GlobalRating = GlobalRating;
            _source.Description = Description;
            _source.ForeignUrl = ForeignUrl;
            _source.PosterUrl = PosterUrl;

            _source.Genres = Genres.Select(g => g.FillModel()).ToList();
            //_source.Genres.Clear();
            //foreach(var genre in Genres)
            //{
            //    genre.FillModel();
            //    _source.Genres.Add(genre.Source);
            //}
            _source.EndEdit();
            return _source;
        }

        protected internal override void FillFromModel()
        {
            OriginalName = _source.OriginalName;
            LocalName = _source.LocalName;
            Year = _source.Year;
            SelfRating = _source.SelfRating;
            GlobalRating = _source.GlobalRating;
            Description = _source.Description;
            ForeignUrl = _source.ForeignUrl;
            PosterUrl = _source.PosterUrl;

            var genres = _source.Genres?.Select(g => new GenreVm(g));
            if (genres != null)
                Genres = new ObservableCollection<GenreVm>(genres);
            else
                Genres = new ObservableCollection<GenreVm>();

            OnPropertyChanged(nameof(Genres));
            //_source.Changed += _source_Changed;
            //Fill();
        }

        //void _source_Changed(object sender, EventArgs e)
        //{
        //    _source.Changed -= _source_Changed;
        //    Fill();
        //    _source.Changed += _source_Changed;
        //}

        //void Fill()
        //{
        //    OriginalName = _source.OriginalName;
        //    LocalName = _source.LocalName;
        //    Year = _source.Year;
        //    SelfRating = _source.SelfRating;
        //    GlobalRating = _source.GlobalRating;
        //    Description = _source.Description;
        //    ForeignUrl = _source.ForeignUrl;
        //    PosterUrl = _source.PosterUrl;

        //    var genres = _source.Genres?.Select(g => new GenreVm(g));
        //    if (genres != null)
        //        Genres = new ObservableCollection<GenreVm>(genres);
        //    else
        //        Genres = new ObservableCollection<GenreVm>();

        //    OnPropertyChanged(nameof(Genres));
        //}
    }
}
