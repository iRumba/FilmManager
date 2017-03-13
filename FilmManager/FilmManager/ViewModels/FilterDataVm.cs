using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore;

namespace FilmManager.ViewModels
{
    public class FilterDataVm<TSource, TValue> : FilterDataVm, INotifyCollectionChanged
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

        public void SetData(IEnumerable<TSource> source)
        {
            SetData(source.Select(s=>(object)s));
        }
    }

    public abstract class FilterDataVm : Notifier, IEnumerable<FilterItemVm>, INotifyCollectionChanged
    {
        string _filterText;
        FilterItemVm _selectedItem;
        //bool _notRaiseValueChanged;
        ObservableCollection<FilterItemVm> _items;
        bool _isStrict;

        public event EventHandler<SelectedValueChangedEventArgs> SelectedValueChanged;
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        protected FilterDataVm(string nonSelectedText, object nonSelectedValue, 
            Func<object,object> valueSelector, Func<object, string> textSelector, Func<object, object, bool> equalityComparer,
            string filterText)
        {
            _items = new ObservableCollection<FilterItemVm>();
            Items = new ReadOnlyObservableCollection<FilterItemVm>(_items);

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

            OnStrictChanged();
            Reset();
            //SelectedItem = _items[0];
            //SetData(null);
        }

        protected Func<object, string> TextSelector { get; }

        protected Func<object, object> ValueSelector { get; }

        protected Func<object, object, bool> EqualityComparer { get; }

        public ReadOnlyObservableCollection<FilterItemVm> Items { get; }

        protected void SetData(IEnumerable<object> data)
        {
            try
            {
                if (data == null)
                    data = new object[0];
                var d = data.ToArray();

                var startInd = 0;
                if (!IsStrict)
                    startInd = 1;

                // Перебираем переданную коллекцию
                for (var i = 0; i < d.Length; i++)
                {
                    var text = TextSelector(d[i]);
                    var value = ValueSelector(d[i]);
                    var currentIndex = i + startInd;

                    // Если в текущей коллекции не хватает элементов, добавляем из переданной текущий элемент
                    if (_items.Count == currentIndex)
                        _items.Add(new FilterItemVm { Text = text, Value = value });
                    // Иначе, если текущие значения по порядку не совпадают...
                    else if (!EqualityComparer(_items[currentIndex].Value, value))
                    {
                        // ... то пытаемся найти элемент в текущей коллекции с таким значением в другом месте
                        var findedItem = _items.FirstOrDefault(item => EqualityComparer(item.Value, value));
                        // Если не нашли, то вставляем новый элемент по текущему индексу
                        if (findedItem == null)
                            _items.Insert(currentIndex, new FilterItemVm { Text = text, Value = value });
                        // Иначе перемещаем найденный элемент на текущий индекс
                        else
                            _items.Move(_items.IndexOf(findedItem), currentIndex);
                    }
                }

                var selectedChanged = false;
                for (var i = d.Length + startInd; i < _items.Count; i++)
                {
                    if (!selectedChanged || EqualityComparer(SelectedItem.Value, _items[i].Value))
                    {
                        SelectedItem = _items.First();
                        selectedChanged = true;
                    }

                    _items.RemoveAt(i);
                }
            }
            catch
            {
                throw;
            }
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

        public bool IsStrict
        {
            get
            {
                return _isStrict;
            }
            set
            {
                if (_isStrict != value)
                {
                    _isStrict = value;
                    OnPropertyChanged(nameof(IsStrict));
                    OnStrictChanged();
                }
            }
        }

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
                    OnPropertyChanged(nameof(SelectedValue));
                    OnSelectedValueChanged(this, _selectedItem?.Value);
                }
            }
        }

        public void Reset()
        {
            SelectedItem = _items[0];
        }

        void OnStrictChanged()
        {
            if (IsStrict)
            {
                if (EqualityComparer(_items[0].Value, NonSelectedValue))
                {
                    _items.RemoveAt(0);
                }
            }
            else
            {
                if (_items.Count == 0 || !EqualityComparer(_items[0].Value, NonSelectedValue))
                {
                    _items.Insert(0, new FilterItemVm { Text = NonSelectedText, Value = NonSelectedValue });
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
