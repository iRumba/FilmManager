using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmParser
{
    public class FilmInfo
    {
        public HtmlValueGetterInfo OriginalName { get; set; }
        public HtmlValueGetterInfo LocalName { get; set; }
        public HtmlValueGetterInfo Year { get; set; }
        public HtmlValueGetterInfo GlobalRating { get; set; }
        public HtmlValueGetterInfo PosterUrl { get; set; }
        public HtmlValueGetterInfo Description { get; set; }
        public HtmlValueGetterInfo Genres { get; set; }
    }
}
