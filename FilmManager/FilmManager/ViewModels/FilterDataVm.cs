using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore;

namespace FilmManager.ViewModels
{
    public class FilterDataVm<TSource, TValue> : FilterDataVm, IEnumerable<FilterItemVm<TValue>>, INotifyCollectionChanged
    {
        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
            string filterText, string nonSelectedText, TValue nonSelectedValue, IEqualityComparer<TValue> comparer) 
            : base(nonSelectedText,
                  nonSelectedValue,
                  valueSelector==null ? null : new Func<object,object>(s => valueSelector((TSource)s)),
                  textSelector == null? null : new Func<object,string>(s=>textSelector((TSource)s)),
                  comparer == null ? new Func<object, object, bool>((o1, o2) => EqualityComparer<TValue>.Default.Equals((TValue)o1, (TValue)o2)) : new Func<object, object, bool>((o1,o2)=>comparer.Equals((TValue)o1,(TValue)o2)),
                  filterText)
        { }

        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
    string filterText, string nonSelectedText, TValue nonSelectedValue) :
            this(valueSelector, textSelector, filterText, nonSelectedText, nonSelectedValue, null)
        { }

        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
string filterText, string nonSelectedText) :
    this(valueSelector, textSelector, filterText, nonSelectedText, default(TValue))
        { }

        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
string filterText) :
    this(valueSelector, textSelector, filterText, (string)null)
        { }

        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
string filterText, string nonSelectedText, IEqualityComparer<TValue> comparer) :
    this(valueSelector, textSelector, filterText, nonSelectedText, default(TValue), comparer)
        { }

        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
string filterText, IEqualityComparer<TValue> comparer) :
this(valueSelector, textSelector, filterText, null, default(TValue), comparer)
        { }

        public new IEnumerator<FilterItemVm<TValue>> GetEnumerator()
        {
            return (_items as IEnumerable<FilterItemVm<TValue>>)?.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void SetData(IEnumerable<TSource> source)
        {
            SetData(source.Select(s=>(object)s));
        }
    }

    public abstract class FilterDataVm : Notifier, IEnumerable<FilterItemVm>, INotifyCollectionChanged
    {
        string _filterText;
        object _selectedValue;
        FilterItemVm _selectedItem;
        bool _notRaiseValueChanged;
        Func<object, string> _textSelector;
        Func<object, object> _valueSelector;
        Func<object, object, bool> _equalityComparer;

        protected IEnumerable<FilterItemVm> _items;

        public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected FilterDataVm(string nonSelectedText, object nonSelectedValue, 
            Func<object,object> valueSelector, Func<object, string> textSelector, Func<object, object, bool> equalityComparer,
            string filterText)
        {
            if (valueSelector == null || textSelector == null || equalityComparer == null)
                throw new InvalidOperationException("FilterDataVm: valueSelector or textSelector or equalityComparer is null");

            if (nonSelectedText == null)
                nonSelectedText = "Не выбрано";
            NonSelectedText = nonSelectedText;
            NonSelectedValue = nonSelectedValue;
            ValueSelector = valueSelector;
            TextSelector = textSelector;
            EqualityComparer = equalityComparer;
            FilterText = filterText;
            SetData(null);
        }

        protected Func<object, string> TextSelector { get; }

        protected Func<object, object> ValueSelector { get; }

        protected Func<object, object, bool> EqualityComparer { get; }

        protected void SetData(IEnumerable<object> data)
        {
            if (data == null)
                data = new object[0];
            var d = data.ToArray();
            var list = new List<FilterItemVm>();
            if (!IsStrict || d.Length == 0)
                list.Add(new FilterItemVm { Text = NonSelectedText, Value = NonSelectedValue });
            list.AddRange(d.Where(v => ValueSelector(v) != NonSelectedValue).
                Select(o => new FilterItemVm { Text = TextSelector(o), Value = ValueSelector(o) }));
            _items = list;
            _notRaiseValueChanged = true;
            var lastValue = SelectedItem == null ? NonSelectedValue : SelectedItem.Value;
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            ResetTo(lastValue);
            //SelectedItem = _items.First(i => EqualityComparer(i.Value, lastValue));
            _notRaiseValueChanged = false;
        }

        public IEnumerator<FilterItemVm> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public string NonSelectedText { get; }

        public object NonSelectedValue { get; }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool IsStrict { get; set; }

        public string FilterText
        {
            get
            {
                return _filterText;
            }

            set
            {
                if (_filterText != value)
                {
                    _filterText = value;
                    OnPropertyChanged(nameof(FilterText));
                }
            }
        }

        public object SelectedValue
        {
            get
            {
                return SelectedItem?.Value;
            }
        }

        public FilterItemVm SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                if (_selectedItem != value)
                {
                    var oldValue = _selectedItem;
                    
                    _selectedItem = value;
                    OnPropertyChanged(nameof(SelectedItem));
                    if (value != null)
                    {
                        OnPropertyChanged(nameof(SelectedValue));
                        OnSelectedValueChanged(this, _selectedItem?.Value);
                    }

                    //if (value == null)
                    //{
                    //    _notRaiseValueChanged = true;
                    //    Reset();
                    //    _notRaiseValueChanged = false;
                    //}
                }
            }
        }

        public void Reset()
        {
            ResetTo(NonSelectedValue);
        }

        public void ResetTo(object value)
        {
            var findedItem = _items.FirstOrDefault(i => EqualityComparer(i.Value, value));
            if (findedItem == null)
                if (EqualityComparer(value, NonSelectedValue))
                    SelectedItem = _items.First();
                else
                    ResetTo(NonSelectedValue);
            else
                SelectedItem = findedItem;
        }

        void OnSelectedValueChanged(FilterDataVm source, object newValue)
        {
            if (!_notRaiseValueChanged)
                SelectedValueChanged?.Invoke(this, new SelectedValueChangedEventArgs(source, newValue));
        }
    }

    public class SelectedValueChangedEventArgs : EventArgs
    {
        public object NewValue { get; set; }

        public FilterDataVm Source { get; set; }

        public SelectedValueChangedEventArgs(FilterDataVm source, object newValue)
        {
            Source = source;
            NewValue = newValue;
        }
    }
}
