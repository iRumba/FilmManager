using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore;

namespace FilmManager.ViewModels
{
    public class FilterItemVm<TValue> : FilterItemVm
    {
        public new TValue Value
        {
            get
            {
                return (TValue)base.Value;
            }
            set
            {
                base.Value = value;
            }
        }
    }

    public class FilterItemVm : Notifier
    {
        object _value;
        string _text;

        public object Value
        {
            get
            {
                return _value;
            }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public string Text
        {
            get
            {
                return _text;
            }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged(nameof(value));
                }
            }
        }
    }
}
