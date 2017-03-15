using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManagerCore
{
    class LoggerUnsubscriber : IDisposable
    {
        Logger _logger;
        IObserver<LogMessage> _observer;

        internal LoggerUnsubscriber(Logger logger, IObserver<LogMessage> observer)
        {
            _logger = logger;
            _observer = observer;
        }

        public void Dispose()
        {
            _logger.Unsubscribe(_observer);
        }
    }
}
