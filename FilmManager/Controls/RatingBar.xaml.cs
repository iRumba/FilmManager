using System;
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

namespace Controls
{
    [StyleTypedProperty(Property = "ButtonStyle", StyleTargetType = typeof(RatingBarButton))]
    [TemplatePart(Name = "PART_ButtonsList", Type = typeof(ListBox))]
    public partial class RatingBar : UserControl
    {
        ObservableCollection<RatingBarButton> _buttons;
        ObservableCollection<RatingBarButtonVm> _values;

        public ReadOnlyObservableCollection<RatingBarButton> Buttons { get; }

        public ReadOnlyObservableCollection<RatingBarButtonVm> Values { get; }

        public RatingBar()
        {
            DefaultStyleKey = typeof(RatingBar);
            _buttons = new ObservableCollection<RatingBarButton>();
            Buttons = new ReadOnlyObservableCollection<RatingBarButton>(_buttons);
            _values = new ObservableCollection<RatingBarButtonVm>();
            Values = new ReadOnlyObservableCollection<RatingBarButtonVm>(_values);
            InitializeComponent();
            OnMaxValueChanged();
            OnValueChanged();
        }
        

        public ControlTemplate ButtonTemplate
        {
            get { return (ControlTemplate)GetValue(ButtonTemplateProperty); }
            set { SetValue(ButtonTemplateProperty, value); }
        }

        public static readonly DependencyProperty ButtonTemplateProperty =
            DependencyProperty.Register("ButtonTemplate", typeof(ControlTemplate), typeof(RatingBar), new PropertyMetadata(default(ControlTemplate)));



        public double ButtonWidth
        {
            get { return (double)GetValue(ButtonWidthProperty); }
            set { SetValue(ButtonWidthProperty, value); }
        }

        public static readonly DependencyProperty ButtonWidthProperty =
            DependencyProperty.Register("ButtonWidth", typeof(double), typeof(RatingBar), new PropertyMetadata(20D));



        public double ButtonHeight
        {
            get { return (double)GetValue(ButtonHeightProperty); }
            set { SetValue(ButtonHeightProperty, value); }
        }

        public static readonly DependencyProperty ButtonHeightProperty =
            DependencyProperty.Register("ButtonHeight", typeof(double), typeof(RatingBar), new PropertyMetadata(20D));



        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ButtonStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(RatingBar), new PropertyMetadata(default(Style)));



        public int MaxValue
        {
            get { return (int)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxValue.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxValueProperty =
            DependencyProperty.Register("MaxValue", typeof(int), typeof(RatingBar), 
                new PropertyMetadata(5) { PropertyChangedCallback = MaxValuePropertyChanged },
                ValidateMaxValue);

        static bool ValidateMaxValue(object value)
        {
            if (value is int)
            {
                var intValue = (int)value;
                return intValue > 0;
            }
            return false;
        }

        static void MaxValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ratingBar = d as RatingBar;
            if (ratingBar != null)
                ratingBar.OnMaxValueChanged();
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Value.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(RatingBar), 
                new FrameworkPropertyMetadata(0)
                {
                    CoerceValueCallback = CoerceValue,
                    PropertyChangedCallback = ValuePropertyChanged,
                    BindsTwoWayByDefault = true
                }, 
                ValidateValue);

        static bool ValidateValue(object value)
        {
            if (value is int)
            {
                var intValue = (int)value;
                return intValue >= 0;
            }
            return false;
        }

        static void ValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var ratingBar = d as RatingBar;
            if (ratingBar != null)
                ratingBar.OnValueChanged();
        }

        static object CoerceValue(DependencyObject d, object value)
        {
            var ratingBar = d as RatingBar;
            if (ratingBar!= null && value is int)
            {
                var intValue = (int)value;
                if (intValue < 0)
                    return 0;
                else if (intValue > ratingBar.MaxValue)
                    return ratingBar.MaxValue;
            }
            return value;
        }

        void OnMaxValueChanged()
        {
            var oldValue = Values.Count;
            var delta = MaxValue - oldValue;
            if (delta > 0)
            {
                for(var i = 0; i < delta; i++)
                {
                    var vm = new RatingBarButtonVm { Value = 1 + _values.Count() };
                    CommandManager.RegisterClassCommandBinding(typeof(RatingBar),
                    new CommandBinding(vm.ClickCommand, ButtonClick, ButtonCanClick));
                    _values.Add(vm);
                    
                    //var but = new RatingBarButton();
                    //but.Click += But_Click;
                    //_buttons.Add(but);
                }
            }
            else if (delta < 0)
            {
                if (MaxValue > Value)
                    Value = MaxValue;
                while (Values.Count > MaxValue)
                {
                    _values.RemoveAt(Values.Count - 1);
                    //var but = _buttons[_buttons.Count - 1];
                    //but.Click -= But_Click;
                    //_buttons.Remove(but);
                }
            }
        }

        void ButtonClick(object sender, ExecutedRoutedEventArgs e)
        {
            var but = e.OriginalSource as RatingBarButton;
            if (but != null)
            {
                Value = but.Value;
            }
        }

        void ButtonCanClick(object sender, CanExecuteRoutedEventArgs e)
        {
            var but = e.OriginalSource as RatingBarButton;
            if (but != null)
            {
                e.CanExecute = but.Value <= MaxValue && IsEnabled;
            }
        }

        void OnValueChanged()
        {
            for (var i = 0; i < Values.Count; i++)
            {
                //var listItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromIndex(i);
                //listItem.ApplyTemplate();
                //var but = listItem.ContentTemplate.FindName("PART_RatingButton", listItem);
                //var button = (RatingBarButton)(LogicalTreeHelper.FindLogicalNode(listItem, "PART_RatingButton"));
                var before = i < Value;
                Values[i].IsChecked = before;
                //Buttons[i].SetChecked(before);
            }
        }

        private void uc_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
