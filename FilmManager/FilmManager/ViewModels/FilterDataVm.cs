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
        IEqualityComparer<TValue> _comparer;
        Func<TSource, TValue> _valueSelector;
        Func<TSource, string> _textSelector;
        string _nonSelectedText;
        TValue _nonSelectedValue;

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        public FilterDataVm(Func<TSource, TValue> valueSelector, Func<TSource, string> textSelector,
            string filterText, string nonSelectedText, TValue nonSelectedValue, IEqualityComparer<TValue> comparer)
        {
            if (valueSelector == null || textSelector == null)
                throw new InvalidOperationException("FilterDataVm: valueSelector or textSelector is null");
            _valueSelector = valueSelector;
            _textSelector = textSelector;

            if (comparer == null)
                comparer = EqualityComparer<TValue>.Default;

            if (nonSelectedText == null)
                nonSelectedText = "Не выбрано";

            _items = new List<FilterItemVm<TValue>>();
            
            _comparer = comparer;
            _nonSelectedValue = nonSelectedValue;
            _nonSelectedText = nonSelectedText;
            FilterText = filterText;
        }

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

        public new TValue SelectedValue
        {
            get
            {
                return (TValue)base.SelectedValue;
            }
            set
            {
                base.SelectedValue = value;
            }
        }

        public new IEnumerator<FilterItemVm<TValue>> GetEnumerator()
        {
            return IsStrict ? _items.Select(i => (FilterItemVm<TValue>)i).GetEnumerator() : WithNonSelected().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return IsStrict ? _items.GetEnumerator() : WithNonSelected().GetEnumerator();
        }

        public void SetData(IEnumerable<TSource> source)
        {
            //if (source.Any(fi => _comparer.Compare(_valueSelector(fi), _nonSelectedValue) == 0))
            //    throw new InvalidOperationException("FilterDataVm: source has default value element");

            //var list = new List<FilterItemVm<TValue>>();
            //foreach (var item in source)
            //    list.Add(new FilterItemVm<TValue> { Value = _valueSelector(item), Text = _textSelector(item) });
            //_items = list;
            var added = new List<FilterItemVm<TValue>>();
            var deleted = new List<FilterItemVm<TValue>>();
            //var changed = new List<FilterItemVm<TValue>>();
            var equals = new List<FilterItemVm<TValue>>();

            var newItems = source.Select(src => new FilterItemVm<TValue> { Value = _valueSelector(src), Text = _textSelector(src) }).ToList();

            var ind = 0;
            foreach (var item in newItems)
            {
                var eq = _items.FirstOrDefault(i => _comparer.Equals(((FilterItemVm<TValue>)i).Value, item.Value));
                if (eq != null)
                {
                    if (eq.Text != item.Text)
                    {
                        eq.Text = item.Text;
                        //changed.Add((FilterItemVm<TValue>)eq);
                    }
                    equals.Add((FilterItemVm<TValue>)eq);
                }
                else
                {
                    added.Add(item);
                }
                ind++;
            }

            var concat = added.Concat(equals);

            foreach(var item in _items)
            {
                if (!concat.Contains(item))
                {
                    ((List<FilterItemVm<TValue>>)_items).Remove((FilterItemVm<TValue>)item);
                    deleted.Add((FilterItemVm<TValue>)item);
                }
            }

            foreach(var item in added)
            {
                ((List<FilterItemVm<TValue>>)_items).Insert(newItems.IndexOf(item), item);
            }
            //deleted.AddRange(concat.Except(_items).Select(i => (FilterItemVm<TValue>)i));
            //var added = newItems.Except(_items);

            if (added.Count > 0)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, added));

            if (deleted.Count > 0)
                CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, deleted));
             
            //CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            //OnPropertyChanged(nameof(SelectedValue));
        }

        IEnumerable<FilterItemVm<TValue>> WithNonSelected()
        {
            return new List<FilterItemVm<TValue>> { new FilterItemVm<TValue> { Value = _nonSelectedValue, Text = _nonSelectedText } }.Concat(_items.Select(i => (FilterItemVm<TValue>)i));
        }
    }

    public abstract class FilterDataVm : Notifier, IEnumerable<FilterItemVm>
    {
        string _filterText;
        object _selectedValue;

        public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;

        protected IEnumerable<FilterItemVm> _items;

        protected void SetData(IEnumerable<FilterItemVm> data)
        {
            _items = data;
        }

        public IEnumerator<FilterItemVm> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

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
                return _selectedValue;
            }

            set
            {
                if (_selectedValue != value)
                {
                    _selectedValue = value;
                    OnPropertyChanged(nameof(SelectedValue));
                    OnSelectedValueChanged(this, value);
                }
            }
        }

        void OnSelectedValueChanged(FilterDataVm source, object newValue)
        {
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
