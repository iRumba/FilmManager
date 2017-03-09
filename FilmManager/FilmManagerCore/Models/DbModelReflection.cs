using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Models
{
    public abstract class DbModelReflection<TDbModel> : Editable where TDbModel : class
    {
        protected TDbModel _source;

        protected DbModelReflection(TDbModel source)
        {
            _source = source;
            FillFromModel();
        }

        protected internal abstract void FillFromModel();

        internal abstract TDbModel FillModel();

        public TDbModel Source
        {
            get
            {
                return _source;
            }
        }
    }
}
