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
    public class QuerySelectBuilder<T> : IDisposable where T:class
    {
        IQueryable<T> _currentQuery;
        //int? _itemsPerPage;
        //int? _pageNumber;

        int? _totalCountCache;
        //int? _totalPagesCache;

        internal event EventHandler Disposing;

        internal QuerySelectBuilder(IQueryable<T> startQuery)
        {
            _currentQuery = startQuery;
        }

        public bool IsOrdered
        {
            get
            {
                return _currentQuery is IOrderedQueryable;
            }
        }

        public void AddFilter(Expression<Func<T,bool>> filter)
        {
            _currentQuery = _currentQuery.Where(filter);
        }

        public void AddOrder<TKey>(Expression<Func<T,TKey>> order)
        {
            _currentQuery = _currentQuery.OrderBy(order);
        }

        public void AddDescOrder<TKey>(Expression<Func<T, TKey>> order)
        {
            _currentQuery = _currentQuery.OrderBy(order);
        }

        //public void SetPaginate(int itemsPerPage, int pageNumber)
        //{
        //    _itemsPerPage = itemsPerPage;
        //    _pageNumber = pageNumber;
        //    //_currentQuery = _currentQuery.Skip(itemsPerPage * (pageNumber - 1)).Take(itemsPerPage);
        //}

        public IEnumerable<T> GetResult()
        {
            //var res = _currentQuery;
            //if (_pageNumber.HasValue)
            //{
            //    if (!(res is IOrderedQueryable<T>))
            //        res = res.Skip(_itemsPerPage.Value * (_pageNumber.Value - 1)).Take(_itemsPerPage.Value);
            //}
            //var sql = string.Empty;
            //try
            //{
            //    sql = res.ToString(); //((System.Data.Entity.Core.Objects.ObjectQuery)res.AsQueryable()).ToTraceString();
            //}
            //catch { }
            return _currentQuery;
        }

        public IEnumerable<T> GetPage(int itemsPerPage, int pageNumber)
        {
            var res = _currentQuery;
            if (res is IOrderedQueryable<T>)
                res = res.Skip(itemsPerPage * (pageNumber - 1)).Take(itemsPerPage);
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

        public void Dispose()
        {
            Disposing.Invoke(this, new EventArgs());
            GC.SuppressFinalize(this);
        }

        void ClearCache()
        {
            _totalCountCache = null;
            //_totalPagesCache = null;
        }
    }
}
