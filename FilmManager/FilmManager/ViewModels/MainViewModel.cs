using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FilmManagerCore;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class MainViewModel : Notifier
    {
        internal FilmManagerApplication _source;

        FilmVm _selectedFilm;

        ImageVm _imageSource;

        bool _dataLoading;

        public RoutedCommand RefreshCommand { get; set; }
        public RoutedCommand SearchCommand { get; set; }
        public RoutedCommand ClearFiltersCommand { get; set; }
        public RoutedCommand EditFilmCommand { get; set; }
        public RoutedCommand NewFilmCommand { get; set; }
        public RoutedCommand RemoveFilmCommand { get; set; }
        public RoutedCommand ShowImageCommand { get; set; }
        public RoutedCommand HideImageCommand { get; set; }
        public RoutedCommand OpenParsersCommand { get; set; }

        public MainViewModel()
        {
            RefreshCommand = new RoutedCommand();
            SearchCommand = new RoutedCommand();
            ClearFiltersCommand = new RoutedCommand();
            EditFilmCommand = new RoutedCommand();
            NewFilmCommand = new RoutedCommand();
            ShowImageCommand = new RoutedCommand();
            HideImageCommand = new RoutedCommand();
            RemoveFilmCommand = new RoutedCommand();
            OpenParsersCommand = new RoutedCommand();

            _source = new FilmManagerApplication(ConfigurationManager.AppSettings["connectionString"]);
            Filters = new FilmFiltersSet();
            Filters.SelfRatings.SetData(new int[] { 1, 2, 3, 4, 5 });
            Filters.Ratings.SetData(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Filters.GenreChanged += Filters_GenreChanged;
            Filters.YearChanged += Filters_YearChanged;
            Filters.SelfRatingChanged += Filters_SelfRatingChanged;
            Filters.RatingChanged += Filters_RatingChanged;
            CurrentPage = 1;
            ImageSource = new ImageVm();
            Logs = new LogReaderVm();
            _source.Logger.Subscribe(Logs);
        }

        public LogReaderVm Logs { get; }

        public string SearchText
        {
            get
            {
                return _source.Filters.TextFilter;
            }
            set
            {
                if (_source.Filters.TextFilter != value)
                {
                    _source.Filters.TextFilter = value;
                    OnPropertyChanged(nameof(SearchText));
                }
            }
        }

        async void Filters_RatingChanged(object sender, RatingChangedEventArgs e)
        {
            _source.Filters.Rating = e.NewValue;
            await RefreshAsync();
        }

        async void Filters_SelfRatingChanged(object sender, SelfRatingChangedEventArgs e)
        {
            _source.Filters.SelfRating = e.NewValue;
            await RefreshAsync();
        }

        async void Filters_YearChanged(object sender, YearChangedEventArgs e)
        {
            _source.Filters.Year = e.NewValue;
            await RefreshAsync();
        }

        async void Filters_GenreChanged(object sender, GenreChangedEventArgs e)
        {
            _source.Filters.GenreId = e.NewValue;
            await RefreshAsync();
        }

        public MainVmAdditionalData AdditionalData { get; set; }

        public FilmFiltersSet Filters { get; set; }

        public List<FilmVm> Films
        {
            get
            {
                return _source.Films.Select(f=>new FilmVm(f) { AutosaveSelfRating = true }).ToList();
            }
        }

        public int ItemsPerPage
        {
            get
            {
                return _source.ItemsPerPage;
            }
            set
            {
                if (_source.ItemsPerPage != value)
                {
                    _source.ItemsPerPage = value;
                    OnPropertyChanged(nameof(ItemsPerPage));
                    RefreshAsync().ConfigureAwait(false);
                }
            }
        }

        public List<GenreVm> UsedGenres
        {
            get
            {
                return _source.UsedGenres.Select(g=>new GenreVm(g)).ToList();
            }
        }

        public List<string> TestStrings { get; set; } = new List<string> { "qwe", "asd" };

        public int CurrentPage
        {
            get
            {
                return _source.CurrentPage;
            }
            set
            {
                if (_source.CurrentPage != value)
                {
                    _source.CurrentPage = value;
                    RefreshAsync().ConfigureAwait(false);
                }
            }
        }

        public int TotalPages
        {
            get
            {
                return _source.PageCount;
            }
        }

        public bool DataLoading
        {
            get
            {
                return _dataLoading;
            }

            set
            {
                if (_dataLoading != value)
                {
                    _dataLoading = value;
                    OnPropertyChanged(nameof(DataLoading));
                }
            }
        }

        public FilmVm SelectedFilm
        {
            get
            {
                return _selectedFilm;
            }
            set
            {
                if (SelectedFilm != value)
                {
                    _selectedFilm = value;
                    OnPropertyChanged(nameof(SelectedFilm));
                }
            }
        }
        public ImageVm ImageSource
        {
            get
            {
                return _imageSource;
            }

            set
            {
                _imageSource = value;
            }
        }

        public void Refresh()
        {
            DataLoading = true;
            _source.Refresh();
            UpdateValues();
            DataLoading = false;
        }

        public async Task RefreshAsync()
        {
            DataLoading = true;
            await _source.RefreshAsync();
            UpdateValues();
            DataLoading = false;
        }

        //public void RefreshAdditionalData()
        //{
        //    _source.RefreshAdditionalData();

        //}

        void UpdateValues()
        {
            OnPropertyChanged(nameof(Films));
            OnPropertyChanged(nameof(TotalPages));
            OnPropertyChanged(nameof(CurrentPage));
            Filters.Genres.SetData(_source.UsedGenres.Select(g => new GenreVm(g)).ToList());
            Filters.Years.SetData(_source.Years);
        }

        internal void ClearFilters()
        {
            Filters.Genres.Reset();//.SelectedValue = null;
            Filters.Years.Reset();//.SelectedValue = null;
            Filters.SelfRatings.Reset();//.SelectedValue = null;
            Filters.Ratings.Reset();//.SelectedValue = null;
            SearchText = string.Empty;
        }

        internal async Task EditFilmAsync(FilmVm editedFilm = null)
        {
            //var editedFilm = editFilm; //?? SelectedFilm;
            var filmEditWnd = new Wnd_FilmEditing();
            FilmVm filmVm = null;
            if (editedFilm != null)
            {
                filmVm = new FilmVm(editedFilm.Source);
                //filmVm.FillFromModel();
            }
            else
            {
                filmVm = new FilmVm();
            }

            filmEditWnd.Source.Film = filmVm;
            filmEditWnd.Source.AllGenres = _source.AllGenres.Select(g => new GenreVm(g)).ToList();
            if (filmEditWnd.ShowDialog() == true)
            {
                filmVm.FillModel();
                //editedFilm.FillModel();
                if (editedFilm == null)
                    await _source.AddOrUpdateFilmsAsync(filmVm.Source);
            }
        }

        //public async Task AddFilmAsync()
        //{
        //    var filmEditWnd = new Wnd_FilmEditing();
        //    filmEditWnd.Source.AllGenres = _source.AllGenres.Select(g => new GenreVm(g)).ToList();
        //    if (filmEditWnd.ShowDialog() == true)
        //    {
        //        await _source.AddOrUpdateFilmsAsync(filmEditWnd.Source.Film);
        //    }
        //}

        public async Task RemoveFilmAsync(Film film)
        {
            await _source.RemoveFilmsAsync(film);
        }
    }
}
