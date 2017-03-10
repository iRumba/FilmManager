using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.SqliteUtils
{
    [SQLiteFunction("CHARINDEX", 2, FunctionType.Scalar)]
    class SqliteCharindexFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            var s1 = args[0] == DBNull.Value ? string.Empty : (string)args[0];
            var s2 = args[1] == DBNull.Value ? string.Empty : (string)args[1];
            return s2.IndexOf(s1, StringComparison.CurrentCulture) + 1;
        }
    }
}
