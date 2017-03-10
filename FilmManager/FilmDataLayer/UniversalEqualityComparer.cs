using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer
{
    public class UniversalEqualityComparer<T> : IEqualityComparer<T>
    {
        Func<T, T, bool> _comparer;
        Func<T, int> _hashCodeGetter;

        public UniversalEqualityComparer(Func<T, T, bool> predicate, Func<T, int> hashCodeGetter)
        {
            _comparer = predicate;
            _hashCodeGetter = hashCodeGetter;
        }

        public bool Equals(T x, T y)
        {
            return _comparer?.Invoke(x, y) ?? false;
        }

        public int GetHashCode(T obj)
        {
            return _hashCodeGetter?.Invoke(obj) ?? default(int);
        }
    }
}
