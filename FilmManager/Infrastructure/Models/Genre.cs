using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Genre
    {
        public long GenreId { get; set; }
        public string Name { get; set; }
        public virtual List<Film> Films { get; set; }
    }
}
