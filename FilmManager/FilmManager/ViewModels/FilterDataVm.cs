using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManager.ViewModels
{
    public class FilterDataVm : IEnumerable<FilterItemVm>
    {
        IEnumerable<FilterItemVm> _items;

        public FilterDataVm(IEnumerable<FilterItemVm> data)
        {
            if (data.Any(fi => fi.Value == null))
                throw new InvalidOperationException("FilterDataVm");
            var list = new List<FilterItemVm>();
            list.Add(new FilterItemVm { Value = null, Text = "Не выбрано" });
            list.AddRange(data);
            _items = list;
        }

        public IEnumerator<FilterItemVm> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
    }
}
