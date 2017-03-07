using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using FilmManagerCore;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class FilmEditVm : Notifier
    {
        FilmVm _film;
        List<GenreVm> _allGenres;
        ListCollectionView _genresForChange;
        //SearchComparer _searchComparer;
        bool _genreTextBoxFocused;
        string _genreText;

        public RoutedCommand RemoveTagCommand { get; set; }
        public RoutedCommand AddTagCommand { get; set; }

        public FilmEditVm()
        {
            RemoveTagCommand = new RoutedCommand();
            AddTagCommand = new RoutedCommand();
            //_searchComparer = new SearchComparer();
        }

        public FilmVm Film
        {
            get
            {
                if (_film == null)
                    _film = new FilmVm();
                return _film;
            }
            set
            {
                if (_film != value)
                {
                    _film = value;
                    OnPropertyChanged(nameof(Film));
                }
            }
        }

        //public string OriginalName
        //{
        //    get
        //    {
        //        return Film.OriginalName;
        //    }
        //    set
        //    {
        //        if (Film.OriginalName != value)
        //        {
        //            Film.OriginalName = value;
        //            OnPropertyChanged(nameof(OriginalName));
        //        }
        //    }
        //}

        //public string LocalName
        //{
        //    get
        //    {
        //        return Film.LocalName;
        //    }
        //    set
        //    {
        //        if (Film.LocalName != value)
        //        {
        //            Film.LocalName = value;
        //            OnPropertyChanged(nameof(LocalName));
        //        }
        //    }
        //}

        //public string Description
        //{
        //    get
        //    {
        //        return Film.Description;
        //    }
        //    set
        //    {
        //        if (Film.Description != value)
        //        {
        //            Film.Description = value;
        //            OnPropertyChanged(nameof(Description));
        //        }
        //    }
        //}

        //public string ForeignUrl
        //{
        //    get
        //    {
        //        return Film.ForeignUrl;
        //    }
        //    set
        //    {
        //        if (Film.ForeignUrl != value)
        //        {
        //            Film.ForeignUrl = value;
        //            OnPropertyChanged(nameof(ForeignUrl));
        //        }
        //    }
        //}

        //public string PosterUrl
        //{
        //    get
        //    {
        //        return Film.PosterUrl;
        //    }
        //    set
        //    {
        //        if (Film.PosterUrl != value)
        //        {
        //            Film.PosterUrl = value;
        //            OnPropertyChanged(nameof(PosterUrl));
        //        }
        //    }
        //}

        //public int? Year
        //{
        //    get
        //    {
        //        return Film.Year;
        //    }
        //    set
        //    {
        //        if (Film.Year != value)
        //        {
        //            Film.Year = value;
        //            OnPropertyChanged(nameof(Year));
        //        }
        //    }
        //}

        //public float? GlobalRating
        //{
        //    get
        //    {
        //        return Film.GlobalRating;
        //    }
        //    set
        //    {
        //        if (Film.GlobalRating != value)
        //        {
        //            Film.GlobalRating = value;
        //            OnPropertyChanged(nameof(GlobalRating));
        //        }
        //    }
        //}

        //public int SelfRating
        //{
        //    get
        //    {
        //        return Film.SelfRating;
        //    }
        //    set
        //    {
        //        if (Film.SelfRating != value)
        //        {
        //            Film.SelfRating = value;
        //            OnPropertyChanged(nameof(SelfRating));
        //        }
        //    }
        //}

        //public ObservableCollection<Genre> Genres
        //{
        //    get
        //    {
        //        return Film.Genres;
        //    }
        //}

        public List<GenreVm> AllGenres
        {
            get
            {
                return _allGenres;
            }

            set
            {
                _allGenres = value;
                //GenresForChange = new ListCollectionView(value.Except(Genres).ToList());
                OnPropertyChanged(nameof(AllGenres));
            }
        }

        public ListCollectionView GenresForChange
        {
            get
            {
                return _genresForChange;
            }

            set
            {
                _genresForChange = value;
                _genresForChange.IsLiveFiltering = _genresForChange.CanChangeLiveFiltering;
                _genresForChange.Filter = (o) => !Film.Genres.Contains(o);
                OnPropertyChanged(nameof(GenresForChange));
            }
        }

        //public string SearchString
        //{
        //    get
        //    {
        //        return _searchComparer.SearchString;
        //    }
        //    set
        //    {
        //        if (_searchComparer.SearchString != value)
        //        {
        //            _searchComparer.SearchString = value;
        //            OnPropertyChanged(nameof(SearchString));
        //            OnPropertyChanged(nameof(IsPopupVisible));
        //            //OnPropertyChanged(nameof(GenresForChange));
        //        }
        //    }
        //}

        //public bool IsPopupVisible
        //{
        //    get
        //    {
        //        return GenreTextBoxFocused && SearchString.Length > 0;
        //    }
        //}

        //public bool GenreTextBoxFocused
        //{
        //    get
        //    {
        //        return _genreTextBoxFocused;
        //    }

        //    set
        //    {
        //        _genreTextBoxFocused = value;
        //        OnPropertyChanged(nameof(IsPopupVisible));
        //    }
        //}

        public string GenreText
        {
            get
            {
                return _genreText;
            }

            set
            {
                if (_genreText == null)
                    _genreText = string.Empty;
                if (!_genreText.Equals(value, StringComparison.CurrentCultureIgnoreCase))
                {
                    _genreText = value;
                    OnPropertyChanged(nameof(GenreText));
                }
                
            }
        }
    }

    //class SearchComparer : IComparer<string>
    //{
    //    public string SearchString { get; set; }

    //    public int Compare(string x, string y)
    //    {
    //        var xIndex = x.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase);
    //        var yIndex = y.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase);

    //        if (xIndex < 0 && yIndex < 0)
    //            return 0;
    //        if (xIndex < 0)
    //            return xIndex;
    //        if (yIndex < 0)
    //            return -yIndex;
    //        return x.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase)
    //            .CompareTo(y.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase));
    //    }
    //}
}
