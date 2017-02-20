using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FilmDataLayer.Models;

namespace FilmDataLayer
{
    public class QuerySelectBuilder<T> : IDisposable
    {
        IQueryable<T> _currentQuery;
        int? _itemsPerPage;
        int? _pageNumber;

        int? _totalCountCache;
        int? _totalPagesCache;

        internal event EventHandler Disposing;

        internal QuerySelectBuilder(IQueryable<T> startQuery)
        {
            _currentQuery = startQuery;
        }

        public void AddFilter(Expression<Func<T,bool>> filter)
        {
            _currentQuery = _currentQuery.Where(filter);
            ClearCache();
        }

        public void AddOrder<TKey>(Expression<Func<T,TKey>> order)
        {
            _currentQuery = _currentQuery.OrderBy(order);
            ClearCache();
        }

        public void AddDescOrder<TKey>(Expression<Func<T, TKey>> order)
        {
            _currentQuery = _currentQuery.OrderBy(order);
            ClearCache();
        }

        public void SetPaginate(int itemsPerPage, int pageNumber)
        {
            _itemsPerPage = itemsPerPage;
            _pageNumber = pageNumber;
            //_currentQuery = _currentQuery.Skip(itemsPerPage * (pageNumber - 1)).Take(itemsPerPage);
        }

        public IEnumerable<T> GetResult()
        {
            var res = _currentQuery;
            if (_pageNumber.HasValue)
                res = res.Skip(_itemsPerPage.Value * (_pageNumber.Value - 1)).Take(_itemsPerPage.Value);
            return res;
        }

        public int TotalCount
        {
            get
            {
                if (!_totalCountCache.HasValue)
                    _totalCountCache = _currentQuery.Count();
                return _totalCountCache.Value;
            }
        }

        public int? TotalPages
        {
            get
            {
                if (_itemsPerPage.HasValue)
                {
                    if (!_totalPagesCache.HasValue)
                    {
                        var tCount = TotalCount;
                        _totalPagesCache = (int)Math.Ceiling((decimal)tCount / _itemsPerPage.Value);
                    }
                    return _totalPagesCache;
                }
                return null;
            }
        }

        public int? CurrentPage
        {
            get
            {
                if (_itemsPerPage.HasValue)
                {
                    return Math.Min(_pageNumber.Value, TotalPages.Value);
                }
                return null;
            }
        }

        public void Dispose()
        {
            Disposing.Invoke(this, new EventArgs());
            GC.SuppressFinalize(this);
        }

        void ClearCache()
        {
            _totalCountCache = null;
            _totalPagesCache = null;
        }
    }
}
