using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Parsing;

namespace FilmManager.ViewModels
{
    public class ParserVm : ModelReflection<Parser>
    {
        public ParserVm(Parser source) : base(source)
        {
        }

        protected internal override void FillFromModel()
        {
            throw new NotImplementedException();
        }

        protected internal override Parser FillModel()
        {
            throw new NotImplementedException();
        }
    }
}
