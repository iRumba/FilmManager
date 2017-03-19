using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FilmManagerCore.Models
{
    public class LogMessage
    {
        public LogMessage()
        {
            Date = DateTime.Now;
        }
        public DateTime Date { get; }
        public string Type { get; set; }
        public string Message { get; set; }
    }
}
