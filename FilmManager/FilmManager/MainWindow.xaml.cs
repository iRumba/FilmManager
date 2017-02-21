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
    }
}
