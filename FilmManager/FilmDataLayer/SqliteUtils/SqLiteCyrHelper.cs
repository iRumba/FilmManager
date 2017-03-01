using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.SqliteUtils
{
    [SQLiteFunction(Name = "CYR_UPPER", Arguments = 1, FuncType = FunctionType.Scalar)]
    class SqLiteCyrHelper : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return args[0] != null ? ((string)args[0]).ToUpper() : null;
        }
    }
}
