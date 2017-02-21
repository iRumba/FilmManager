using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class MainVmAdditionalData : Notifier
    {
        public List<Genre> Genres { get; set; }
    }
}
