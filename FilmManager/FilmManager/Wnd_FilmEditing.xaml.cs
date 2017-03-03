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
using System.Windows.Shapes;
using FilmManager.ViewModels;

namespace FilmManager
{
    /// <summary>
    /// Логика взаимодействия для Wnd_FilmEditing.xaml
    /// </summary>
    public partial class Wnd_FilmEditing : Window
    {
        public Wnd_FilmEditing()
        {
            InitializeComponent();
        }

        public FilmEditVm Source
        {
            get
            {
                return DataContext as FilmEditVm;
            }
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
