using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManagerCore
{
    public abstract class LogBaseObserver : IObserver<LogMessage>
    {
        public List<string> LogTypes { get; }

        protected LogBaseObserver(string[] logTypes)
        {
            LogTypes = new List<string>(logTypes);
        }

        protected LogBaseObserver() : this(new string[0]) { }

        public abstract void OnCompleted();

        public abstract void OnError(Exception error);

        public abstract void OnNext(LogMessage value);
    }
}
