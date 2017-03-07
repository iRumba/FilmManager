using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FilmManager.ViewModels;
using FilmManagerCore;
using FilmManagerCore.Models;

namespace FilmManager
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {

            InitializeComponent();
            var refreshCommand = new CommandBinding(Source.RefreshCommand);
            refreshCommand.CanExecute += RefreshCommand_CanExecute;
            refreshCommand.Executed += RefreshCommand_Executed;

            var searchCommand = new CommandBinding(Source.SearchCommand);
            searchCommand.CanExecute += SearchCommand_CanExecute;
            searchCommand.Executed += SearchCommand_Executed;

            var clearFilters = new CommandBinding(Source.ClearFiltersCommand);
            clearFilters.CanExecute += ClearFiltersCommand_CanExecute;
            clearFilters.Executed += ClearFiltersCommand_Executed;

            var editFilm = new CommandBinding(Source.EditFilmCommand);
            editFilm.CanExecute += EditFilm_CanExecute;
            editFilm.Executed += EditFilm_Executed;

            var newFilm = new CommandBinding(Source.NewFilmCommand);
            newFilm.CanExecute += NewFilm_CanExecute;
            newFilm.Executed += NewFilm_Executed;

            var showImage = new CommandBinding(Source.ShowImageCommand);
            showImage.CanExecute += ShowImage_CanExecute;
            showImage.Executed += ShowImage_Executed;

            var hideImage = new CommandBinding(Source.HideImageCommand);
            hideImage.CanExecute += HideImage_CanExecute;
            hideImage.Executed += HideImage_Executed;

            var removeFilm = new CommandBinding(Source.RemoveFilmCommand);
            removeFilm.CanExecute += RemoveFilm_CanExecute;
            removeFilm.Executed += RemoveFilm_Executed;

            CommandManager.RegisterClassCommandBinding(GetType(), refreshCommand);
            CommandManager.RegisterClassCommandBinding(GetType(), searchCommand);
            CommandManager.RegisterClassCommandBinding(GetType(), clearFilters);
            CommandManager.RegisterClassCommandBinding(GetType(), editFilm);
            CommandManager.RegisterClassCommandBinding(GetType(), newFilm);
            CommandManager.RegisterClassCommandBinding(GetType(), showImage);
            CommandManager.RegisterClassCommandBinding(GetType(), hideImage);
            CommandManager.RegisterClassCommandBinding(GetType(), removeFilm);
        }

        async void RemoveFilm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var film = e.Parameter as Film;
            if (film != null)
            {
                await Source.RemoveFilmAsync(film);
                await Source.RefreshAsync();
            }
        }

        void RemoveFilm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            Film param = null;
            e.CanExecute = (param = e.Parameter as Film) != null && param.FilmId != default(long);
            //if (!e.CanExecute)
            //    Console.WriteLine(false);
        }

        void HideImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Source.ImageSource.Hide();
        }

        void HideImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        async void ShowImage_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string)
            {
                await Source.ImageSource.LoadImageAsync((string)e.Parameter);
            }
        }

        void ShowImage_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        async void NewFilm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Source.AddFilmAsync();
            await Source.RefreshAsync();
        }

        void NewFilm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        async void EditFilm_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Source.EditFilmAsync(e.Parameter as Film);
            await Source.RefreshAsync();
        }

        void EditFilm_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            var film = e.Parameter as Film;
            e.CanExecute = film != null;
        }

        async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await Source.RefreshAsync();
        }

        MainViewModel Source
        {
            get
            {
                if (DataContext is MainViewModel)
                    return (MainViewModel)DataContext;
                throw new InvalidOperationException("DataContext required");
            }
        }

        async void ClearFiltersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Source.ClearFilters();
            await Source.RefreshAsync();
        }

        void ClearFiltersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        async void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Source.RefreshAsync();
        }

        void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        async void RefreshCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Source.RefreshAsync();
        }

        void RefreshCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
    }
}
