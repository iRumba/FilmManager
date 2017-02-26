using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Controls
{
    public class RatingBarButtonVm : INotifyPropertyChanged
    {
        int _value;
        bool _isChecked;

        public RoutedUICommand ClickCommand { get; set; } = new RoutedUICommand();

        public int Value
        {
            get
            {
                return _value;
            }
            internal set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public bool IsChecked
        {
            get
            {
                return _isChecked;
            }

            internal set
            {
                if (true || _isChecked != value)
                {
                    _isChecked = value;
                    OnPropertyChanged(nameof(IsChecked));
                }
                
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
