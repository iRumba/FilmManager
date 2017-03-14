using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class FilmListInfo
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public HtmlValueGetterInfo Films { get; set; }
    }
}
