using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.SqliteUtils
{
    [SQLiteFunction("LOWER", 1, FunctionType.Scalar)]
    class SqliteLowerFunction : SQLiteFunction
    {
        public override object Invoke(object[] args)
        {
            return args[0] != DBNull.Value ? ((string)args[0]).ToLowerInvariant() : null;
        }
    }
}
