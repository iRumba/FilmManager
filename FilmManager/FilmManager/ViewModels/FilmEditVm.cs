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
        string _genreText;

        public RoutedCommand RemoveTagCommand { get; set; }
        public RoutedCommand AddTagCommand { get; set; }

        public FilmEditVm()
        {
            RemoveTagCommand = new RoutedCommand();
            AddTagCommand = new RoutedCommand();
        }

        public FilmVm Film
        {
            get
            {
                return _film;
            }
            set
            {
                if (_film != value)
                {
                    if (_film!=null)
                        _film.Genres.CollectionChanged -= Genres_CollectionChanged;
                    _film = value;
                    _film.Genres.CollectionChanged += Genres_CollectionChanged;
                    OnPropertyChanged(nameof(Film));
                    GenresForChange?.Refresh();
                }
            }
        }

        void Genres_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            GenresForChange?.Refresh();
        }

        public ObservableCollection<GenreVm> Genres
        {
            get
            {
                return Film.Genres;
            }
        }

        public List<GenreVm> AllGenres
        {
            get
            {
                return _allGenres;
            }

            set
            {
                _allGenres = value;
                GenresForChange = new ListCollectionView(value);
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
                _genresForChange.Filter = (o) => !Film.Genres.Any(g => ((GenreVm)o).Source.GenreId == g.Source.GenreId);
                OnPropertyChanged(nameof(GenresForChange));
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
}
