using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManager.ViewModels
{
    public abstract class ModelReflection<TModel> : Notifier where TModel : class
    {
        protected TModel _source;

        protected ModelReflection(TModel source)
        {
            _source = source;
            //Init();
        }

        protected internal abstract void FillFromModel();

        protected internal abstract void FillModel();

        protected TModel Source
        {
            get
            {
                return _source;
            }
        }
    }
}
