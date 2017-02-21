using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class MainViewModel : Notifier
    {
        FilmManagerApplication _source;

        public MainViewModel()
        {
            _source = new FilmManagerApplication(ConfigurationManager.AppSettings["connectionString"]);
        }

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
        }
    }
}
