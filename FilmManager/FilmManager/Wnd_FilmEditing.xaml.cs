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
using FilmManagerCore.Models;

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

            var removeTag = new CommandBinding(Source.RemoveTagCommand);
            removeTag.Executed += RemoveTag_Executed;
            removeTag.CanExecute += RemoveTag_CanExecute;

            var addTag = new CommandBinding(Source.AddTagCommand);
            addTag.CanExecute += AddTag_CanExecute;
            addTag.Executed += AddTag_Executed;

            CommandManager.RegisterClassCommandBinding(typeof(Wnd_FilmEditing), removeTag);
            CommandManager.RegisterClassCommandBinding(typeof(Wnd_FilmEditing), addTag);
        }

        void AddTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string)
            {
                var genreName = (string)e.Parameter;
                var finded = Source.AllGenres.Find(g => g.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase));
                if (finded != null)
                {
                    Source.Genres.Add(finded);
                }
                else
                    Source.Genres.Add(new GenreVm { Name = genreName });
                Source.GenreText = string.Empty;
            }
        }

        void AddTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (e.Parameter is string)
            {
                var genreName = (string)e.Parameter;
                if (string.IsNullOrWhiteSpace(genreName))
                    return;

                var finded = Source.Genres.FirstOrDefault(g => g.Name.Equals(genreName, StringComparison.CurrentCultureIgnoreCase));
                if (finded == null)
                    e.CanExecute = true;
            }
        }

        void RemoveTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void RemoveTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is GenreVm)
                Source.Genres.Remove((GenreVm)e.Parameter);
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

        void cmbNewTag_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Genre selectedGenre = null;
            if (e.AddedItems.Count == 1 && (selectedGenre = e.AddedItems[0] as Genre) != null && ((ICommand)Source.AddTagCommand).CanExecute(selectedGenre.Name))
                ((ICommand)Source.AddTagCommand).Execute(selectedGenre.Name);
            ((ComboBox)sender).SelectedItem = null;
        }
    }
}
