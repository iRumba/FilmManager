﻿using System;
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
        FilmManagerApplication _source;

        public RoutedCommand RefreshCommand { get; set; }
        public RoutedCommand SearchCommand { get; set; }
        public RoutedCommand ClearFiltersCommand { get; set; }

        public MainViewModel()
        {
            RefreshCommand = new RoutedCommand();
            SearchCommand = new RoutedCommand();
            ClearFiltersCommand = new RoutedCommand();

            var refreshCommand = new CommandBinding(RefreshCommand);
            refreshCommand.CanExecute += RefreshCommand_CanExecute;
            refreshCommand.Executed += RefreshCommand_Executed;

            var searchCommand = new CommandBinding(SearchCommand);
            searchCommand.CanExecute += SearchCommand_CanExecute;
            searchCommand.Executed += SearchCommand_Executed;

            var clearFilters = new CommandBinding(ClearFiltersCommand);
            clearFilters.CanExecute += ClearFiltersCommand_CanExecute;
            clearFilters.Executed += ClearFiltersCommand_Executed;

            CommandManager.RegisterClassCommandBinding(typeof(MainWindow), refreshCommand);
            CommandManager.RegisterClassCommandBinding(typeof(MainViewModel), searchCommand);
            CommandManager.RegisterClassCommandBinding(typeof(MainViewModel), clearFilters);

            _source = new FilmManagerApplication(ConfigurationManager.AppSettings["connectionString"]);
            //AdditionalData = new MainVmAdditionalData();
            Filters = new FilmFiltersSet();
            Filters.SelfRatings.SetData(new int[] { 1, 2, 3, 4, 5 });
            Filters.Ratings.SetData(new float[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 });
            Filters.GenreChanged += Filters_GenreChanged;
            Filters.YearChanged += Filters_YearChanged;
            Filters.SelfRatingChanged += Filters_SelfRatingChanged;
            Filters.RatingChanged += Filters_RatingChanged;
        }

        void ClearFiltersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ClearFilters();
        }

        void ClearFiltersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

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

        async void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await RefreshAsync();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        async void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await RefreshAsync();
        }

        void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
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

        public List<Film> Films
        {
            get
            {
                return _source.Films;
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

        public List<Genre> UsedGenres
        {
            get
            {
                return _source.UsedGenres;
            }
        }

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

        public void Refresh()
        {
            _source.Refresh();
            UpdateValues();
        }

        public async Task RefreshAsync()
        {
            await _source.RefreshAsync();
            UpdateValues();
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
            Filters.Genres.SetData(_source.UsedGenres);
            Filters.Years.SetData(_source.Years);
        }

        void ClearFilters()
        {
            Filters.Genres.SelectedValue = null;
            Filters.Years.SelectedValue = null;
            Filters.SelfRatings.SelectedValue = null;
            Filters.Ratings.SelectedValue = null;
        }
    }
}
