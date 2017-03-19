using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManagerCore
{
    public interface ILogObserver : IObserver<LogMessage>
    {
        bool IsUniversal { get; }
        bool IsSwitchedOn { get; set; }
        IEnumerable<string> GetLogTypes();
    }
}
