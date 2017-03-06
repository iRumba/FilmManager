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
        Film _source;
        List<Genre> _allGenres;
        ListCollectionView _genresForChange;
        SearchComparer _searchComparer;
        bool _genreTextBoxFocused;
        string _genreText;

        public RoutedCommand RemoveTagCommand { get; set; }
        public RoutedCommand AddTagCommand { get; set; }

        public FilmEditVm()
        {
            RemoveTagCommand = new RoutedCommand();
            AddTagCommand = new RoutedCommand();
            _searchComparer = new SearchComparer();
        }

        public Film Source
        {
            get
            {
                if (_source == null)
                    _source = new Film();
                return _source;
            }
            set
            {
                if (_source != value)
                {
                    _source = value;
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public string OriginalName
        {
            get
            {
                return Source.OriginalName;
            }
            set
            {
                if (Source.OriginalName != value)
                {
                    Source.OriginalName = value;
                    OnPropertyChanged(nameof(OriginalName));
                }
            }
        }

        public string LocalName
        {
            get
            {
                return Source.LocalName;
            }
            set
            {
                if (Source.LocalName != value)
                {
                    Source.LocalName = value;
                    OnPropertyChanged(nameof(LocalName));
                }
            }
        }

        public string Description
        {
            get
            {
                return Source.Description;
            }
            set
            {
                if (Source.Description != value)
                {
                    Source.Description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string ForeignUrl
        {
            get
            {
                return Source.ForeignUrl;
            }
            set
            {
                if (Source.ForeignUrl != value)
                {
                    Source.ForeignUrl = value;
                    OnPropertyChanged(nameof(ForeignUrl));
                }
            }
        }

        public string PosterUrl
        {
            get
            {
                return Source.PosterUrl;
            }
            set
            {
                if (Source.PosterUrl != value)
                {
                    Source.PosterUrl = value;
                    OnPropertyChanged(nameof(PosterUrl));
                }
            }
        }

        public int? Year
        {
            get
            {
                return Source.Year;
            }
            set
            {
                if (Source.Year != value)
                {
                    Source.Year = value;
                    OnPropertyChanged(nameof(Year));
                }
            }
        }

        public float? GlobalRating
        {
            get
            {
                return Source.GlobalRating;
            }
            set
            {
                if (Source.GlobalRating != value)
                {
                    Source.GlobalRating = value;
                    OnPropertyChanged(nameof(GlobalRating));
                }
            }
        }

        public int SelfRating
        {
            get
            {
                return Source.SelfRating;
            }
            set
            {
                if (Source.SelfRating != value)
                {
                    Source.SelfRating = value;
                    OnPropertyChanged(nameof(SelfRating));
                }
            }
        }

        public ObservableCollection<Genre> Genres
        {
            get
            {
                return Source.Genres;
            }
        }

        public List<Genre> AllGenres
        {
            get
            {
                return _allGenres;
            }

            set
            {
                _allGenres = value;
                GenresForChange = new ListCollectionView(value.Except(Genres).ToList());
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
                _genresForChange.Filter = (o) => !Genres.Contains(o) && string.IsNullOrWhiteSpace(SearchString) || ((Genre)o).Name.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase) >= 0;
                OnPropertyChanged(nameof(GenresForChange));
            }
        }

        public string SearchString
        {
            get
            {
                return _searchComparer.SearchString;
            }
            set
            {
                if (_searchComparer.SearchString != value)
                {
                    _searchComparer.SearchString = value;
                    OnPropertyChanged(nameof(SearchString));
                    OnPropertyChanged(nameof(IsPopupVisible));
                    //OnPropertyChanged(nameof(GenresForChange));
                }
            }
        }

        public bool IsPopupVisible
        {
            get
            {
                return GenreTextBoxFocused && SearchString.Length > 0;
            }
        }

        public bool GenreTextBoxFocused
        {
            get
            {
                return _genreTextBoxFocused;
            }

            set
            {
                _genreTextBoxFocused = value;
                OnPropertyChanged(nameof(IsPopupVisible));
            }
        }

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

    class SearchComparer : IComparer<string>
    {
        public string SearchString { get; set; }

        public int Compare(string x, string y)
        {
            var xIndex = x.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase);
            var yIndex = y.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase);

            if (xIndex < 0 && yIndex < 0)
                return 0;
            if (xIndex < 0)
                return xIndex;
            if (yIndex < 0)
                return -yIndex;
            return x.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase)
                .CompareTo(y.IndexOf(SearchString, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
