using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmDataLayer.SqliteUtils
{
    [SQLiteFunction(Name = "UTF8CI", FuncType = FunctionType.Collation, Arguments = 2)]
    class SQLiteCaseInsensitiveCollation : SQLiteFunction
    {
        private static readonly System.Globalization.CultureInfo _cultureInfo =
    System.Globalization.CultureInfo.CreateSpecificCulture("ru-RU");

        public override int Compare(string x, string y)
        {
            return string.Compare(x, y, false, _cultureInfo);
        }
    }
}
