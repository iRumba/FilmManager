using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FilmDataLayer.Models
{
    public class Genre
    {
        public long GenreId { get; set; }
        public string Name { get; set; }
        [XmlIgnore]
        public virtual List<Film> Films { get; set; }
        public Genre()
        {
            Films = new List<Film>();
        }
    }
}
