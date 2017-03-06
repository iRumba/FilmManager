using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace FilmManager
{
    /// <summary>
    /// Логика взаимодействия для TagControl.xaml
    /// </summary>
    public partial class TagControl : UserControl, INotifyPropertyChanged
    {
        public RoutedCommand RemoveTagCommand { get; set; }
        public RoutedCommand AddTagCommand { get; set; }
        public TagControl()
        {
            RemoveTagCommand = new RoutedCommand();
            AddTagCommand = new RoutedCommand();

            var removeTag = new CommandBinding(RemoveTagCommand);
            removeTag.Executed += RemoveTag_Executed;
            removeTag.CanExecute += RemoveTag_CanExecute;

            var addTag = new CommandBinding(AddTagCommand);
            addTag.CanExecute += AddTag_CanExecute;
            addTag.Executed += AddTag_Executed;


            InitializeComponent();
            CommandManager.RegisterClassCommandBinding(typeof(TagControl), removeTag);
            CommandManager.RegisterClassCommandBinding(typeof(TagControl), addTag);
            
        }

        void RemoveTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        void AddTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string)
            {
                var tag = (string)e.Parameter;
                SelectedTags.Add(tag);
                cmbNewTag.Text = string.Empty;
            }
        }

        void AddTag_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = false;
            if (e.Parameter is string)
            {
                var tag = (string)e.Parameter;
                if (!string.IsNullOrWhiteSpace(tag))
                    e.CanExecute = true;
            }
        }

        void RemoveTag_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Parameter is string)
            {
                var tag = (string)e.Parameter;
                var col = new List<string>();
                SelectedTags.Remove(tag);
                col.AddRange(SelectedTags);
                SelectedTags = col;
                OnPropertyChanged(nameof(SelectedTags));
            }
        }

        public bool AllowAdding
        {
            get { return (bool)GetValue(AllowAddingProperty); }
            set { SetValue(AllowAddingProperty, value); }
        }

        public static readonly DependencyProperty AllowAddingProperty =
            DependencyProperty.Register("AllowAdding", typeof(bool), typeof(TagControl), new PropertyMetadata(true));



        public IEnumerable<string> AllTags
        {
            get { return (IEnumerable<string>)GetValue(AllTagsProperty); }
            set { SetValue(AllTagsProperty, value); }
        }

        public static readonly DependencyProperty AllTagsProperty =
            DependencyProperty.Register("AllTags", typeof(IEnumerable<string>), typeof(TagControl), new PropertyMetadata(null));



        public IList<string> SelectedTags
        {
            get { return (IList<string>)GetValue(SelectedTagsProperty); }
            set { SetValue(SelectedTagsProperty, value); }
        }

        public static readonly DependencyProperty SelectedTagsProperty =
            DependencyProperty.Register("SelectedTags", typeof(IList<string>), typeof(TagControl), new PropertyMetadata(null));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TagControl), new PropertyMetadata(string.Empty));

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
