using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Parsing
{
    public class HtmlValueGetterInfo
    {
        public string ElementSearchingString { get; set; }

        public string Attribute { get; set; }
        public string RegexMatch { get; set; }

        public bool UseRegex { get; set; }
    }
}
