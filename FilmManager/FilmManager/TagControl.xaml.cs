using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class TagControl : UserControl
    {
        public TagControl()
        {
            InitializeComponent();
        }



        public bool AllowAdding
        {
            get { return (bool)GetValue(AllowAddingProperty); }
            set { SetValue(AllowAddingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllowAdding.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllowAddingProperty =
            DependencyProperty.Register("AllowAdding", typeof(bool), typeof(TagControl), new PropertyMetadata(true));



        public IEnumerable<string> AllTags
        {
            get { return (IEnumerable<string>)GetValue(AllTagsProperty); }
            set { SetValue(AllTagsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for AllTags.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AllTagsProperty =
            DependencyProperty.Register("AllTags", typeof(IEnumerable<string>), typeof(TagControl), new PropertyMetadata(null));



        public IList<string> SelectedTags
        {
            get { return (IList<string>)GetValue(SelectedTagsProperty); }
            set { SetValue(SelectedTagsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedTags.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedTagsProperty =
            DependencyProperty.Register("SelectedTags", typeof(IList<string>), typeof(TagControl), new PropertyMetadata(null));



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TagControl), new PropertyMetadata(string.Empty));


    }
}
