﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FilmManagerCore.Models
{
    public class Genre
    {
        [DataMember]
        public int GenreId { get; set; }
        [DataMember]
        public string Name { get; set; }
        [XmlIgnore]
        public List<Film> Films { get; set; }
        public Genre()
        {
            Films = new List<Film>();
        }
    }
}
