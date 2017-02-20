using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Models
{
    [DataContract]
    public class Genre
    {
        [DataMember]
        public int GenreId { get; internal set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public ICollection<Film> Films { get; set; }
        public Genre()
        {
            Films = new List<Film>();
        }
    }
}
