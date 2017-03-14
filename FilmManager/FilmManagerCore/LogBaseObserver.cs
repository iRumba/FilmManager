using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManagerCore
{
    public class LogBaseObserver : IObserver<LogMessage>
    {
        public List<string> LogTypes { get; }

        public LogBaseObserver(string[] logTypes)
        {
            LogTypes = new List<string>(logTypes);
        }

        public LogBaseObserver() : this(new string[0])
        {

        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(LogMessage value)
        {
            throw new NotImplementedException();
        }
    }
}
