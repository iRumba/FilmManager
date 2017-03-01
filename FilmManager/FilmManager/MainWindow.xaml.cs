using System;
using System.Collections.Generic;
using System.Linq;
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

            CommandManager.RegisterClassCommandBinding(GetType(), refreshCommand);
            CommandManager.RegisterClassCommandBinding(GetType(), searchCommand);
            CommandManager.RegisterClassCommandBinding(GetType(), clearFilters);
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

        void ClearFiltersCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Source.ClearFilters();
        }

        void ClearFiltersCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        async void SearchCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            await Source.RefreshAsync();
        }

        private void SearchCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
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
