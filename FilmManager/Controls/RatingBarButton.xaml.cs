using System.Windows;
using System.Windows.Controls.Primitives;

namespace Controls
{
    public partial class RatingBarButton : ButtonBase
    {
        bool? _afterLoadingSet;

        public RatingBarButton()
        {
            DefaultStyleKey = typeof(RatingBarButton);
            InitializeComponent();
        }

        internal void SetChecked(bool value)
        {
            if (IsLoaded)
            {
                var visualState = value ? "Checked" : "Unchecked";
                VisualStateManager.GoToState(this, visualState, true);
            }
            else
                _afterLoadingSet = value;
        }

        public int Value
        {
            get { return (int)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(int), typeof(RatingBarButton), new PropertyMetadata(0));



        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set { SetValue(IsCheckedProperty, value); }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(RatingBarButton), new PropertyMetadata(false) { PropertyChangedCallback = IsCheckedPropertyChanged});

        private static void IsCheckedPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var but = d as RatingBarButton;
            if (d != null && e.NewValue is bool)
            {
                but.SetChecked((bool)e.NewValue);
                
            }
        }

        private void ButtonBase_Loaded(object sender, RoutedEventArgs e)
        {
            if (_afterLoadingSet.HasValue)
                SetChecked(_afterLoadingSet.Value);
        }
    }
}
