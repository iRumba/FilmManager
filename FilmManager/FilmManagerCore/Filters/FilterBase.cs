using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Filters
{
    public abstract class FilterBase
    {
        public abstract IQueryable<T> ApplyFilter<T>(IQueryable<T> query);
    }
}
