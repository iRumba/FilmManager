using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Models
{
    public abstract class DbModelReflection<TDbModel> where TDbModel : class
    {
        TDbModel _source;

        protected DbModelReflection(TDbModel source)
        {
            _source = source;
        }

        protected internal abstract TDbModel GetDbModel();
    }
}
