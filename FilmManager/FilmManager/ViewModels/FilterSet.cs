using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManager.ViewModels
{
    public class FilterSet : IEnumerable<FilterDataVm>
    {
        Dictionary<string, FilterDataVm> _items;

        public FilterSet()
        {
            _items = new Dictionary<string, FilterDataVm>();
        }

        public FilterDataVm this[string key]
        {
            get
            {
                return _items[key];
            }
            set
            {
                if (!_items.ContainsKey(key) || _items[key] != value)
                {
                    _items[key] = value;
                }
            }
        }

        public IEnumerator<FilterDataVm> GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.Values.GetEnumerator();
        }
    }


}
