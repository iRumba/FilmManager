using System;
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
    /// Логика взаимодействия для PageCounter.xaml
    /// </summary>
    public partial class PageCounter : UserControl, INotifyPropertyChanged
    {
        ObservableCollection<int> _visiblePages;
        public PageCounter()
        {
            _visiblePages = new ObservableCollection<int>();
            VisiblePages = new ReadOnlyObservableCollection<int>(_visiblePages);
            InitializeComponent();
        }



        public int PagesCount
        {
            get { return (int)GetValue(PagesCountProperty); }
            set { SetValue(PagesCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PagesCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PagesCountProperty =
            DependencyProperty.Register("PagesCount", typeof(int), typeof(PageCounter), new FrameworkPropertyMetadata(1)
            { CoerceValueCallback = PagesCountCoerce,PropertyChangedCallback = PagesCountChanged });

        static void PagesCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageCounter = d as PageCounter;
            if (pageCounter != null)
            {
                pageCounter.OnPagesCountChanged();
            }
        }

        static object PagesCountCoerce(DependencyObject d, object baseValue)
        {
            var pageCounter = d as PageCounter;
            if (pageCounter != null)
            {
                if (baseValue is int)
                {
                    var val = (int)baseValue;
                    if (val < 1)
                        return 1;
                    if (pageCounter.CurrentPage > val)
                        pageCounter.CurrentPage = val;
                }
            }
            return baseValue;
        }

        public int CurrentPage
        {
            get { return (int)GetValue(CurrentPageProperty); }
            set { SetValue(CurrentPageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CurrentPage.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register("CurrentPage", typeof(int), typeof(PageCounter), new FrameworkPropertyMetadata(1)
            { CoerceValueCallback = CurrentPageCoerce, PropertyChangedCallback = CurrentPageChanged, BindsTwoWayByDefault = true });

        static void CurrentPageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageCounter = d as PageCounter;
            if (pageCounter != null)
            {
                //Console.WriteLine(pageCounter.CurrentPage);
                pageCounter.OnCurrentPageChanged();
            }
        }

        static object CurrentPageCoerce(DependencyObject d, object baseValue)
        {
            var pageCounter = d as PageCounter;
            if (pageCounter != null)
            {
                if (baseValue is int)
                {
                    var val = (int)baseValue;
                    if (val < 1)
                        return 1;
                    if (val > pageCounter.PagesCount)
                        return pageCounter.PagesCount;
                }
            }
            return baseValue;
        }

        public int VisiblePagesCount
        {
            get { return (int)GetValue(VisiblePagesCountProperty); }
            set { SetValue(VisiblePagesCountProperty, value); }
        }

        // Using a DependencyProperty as the backing store for VisiblePagesCount.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisiblePagesCountProperty =
            DependencyProperty.Register("VisiblePagesCount", typeof(int), typeof(PageCounter), new FrameworkPropertyMetadata(2)
            { PropertyChangedCallback = VisiblePagesCountChanged, BindsTwoWayByDefault = true });

        public event PropertyChangedEventHandler PropertyChanged;

        static void VisiblePagesCountChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var pageCounter = d as PageCounter;
            if (pageCounter != null)
            {
                pageCounter.OnVisiblePagesCountChanged();
            }
        }

        public ReadOnlyObservableCollection<int> VisiblePages { get; }

        int FirstVisible
        {
            get
            {
                return VisiblePages.FirstOrDefault();
            }
        }

        int LastVisible
        {
            get
            {
                return VisiblePages.LastOrDefault();
            }
        }

        public int ActualFirstVisible
        {
            get
            {
                return Math.Max(CurrentPage - VisiblePagesCount, 1);
            }
        }

        public int ActualLastVisible
        {
            get
            {
                return Math.Min(CurrentPage + VisiblePagesCount, PagesCount);
            }
        }

        public bool FirstButtonEnabled
        {
            get
            {
                return ActualFirstVisible > 1;
            }
        }

        public bool LastButtonEnabled
        {
            get
            {
                return ActualLastVisible < PagesCount;
            }
        }

        void OnCurrentPageChanged()
        {
            RebuildVisiblePages();
        }

        void OnPagesCountChanged()
        {
            RebuildVisiblePages();
        }

        void OnVisiblePagesCountChanged()
        {
            RebuildVisiblePages();
        }

        void RebuildVisiblePages()
        {
            var fv = FirstVisible;
            var lv = LastVisible;
            var afv = ActualFirstVisible;
            var alv = ActualLastVisible;

            if (fv < afv)
            {
                for (var i = fv; i < afv; i++)
                    _visiblePages.Remove(i);
                OnActualFirstVisibleChanged();
            }
            else if (fv > afv)
            {
                for (var i = fv - 1; i >= afv; i--)
                    _visiblePages.Insert(0, i);
                OnActualFirstVisibleChanged();
            }

            if (lv < alv)
            {
                for (var i = lv + 1; i <= alv; i++)
                    _visiblePages.Add(i);
                OnActualLastVisibleChanged();
            }
            else if (lv > alv)
            {
                for (var i = alv + 1; i <= lv; i++)
                    _visiblePages.Remove(i);
                OnActualLastVisibleChanged();
            }
        }

        void OnActualFirstVisibleChanged()
        {
            OnPropertyChanged(nameof(ActualFirstVisible));
            OnPropertyChanged(nameof(FirstButtonEnabled));
        }

        void OnActualLastVisibleChanged()
        {
            OnPropertyChanged(nameof(ActualLastVisible));
            OnPropertyChanged(nameof(LastButtonEnabled));
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        void uc_Loaded(object sender, RoutedEventArgs e)
        {
            RebuildVisiblePages();
        }

        void Button_Click(object sender, RoutedEventArgs e)
        {
            CurrentPage = 1;
        }

        void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CurrentPage = PagesCount;
        }
    }
}
