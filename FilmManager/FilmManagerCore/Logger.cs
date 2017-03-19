using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore.Models;

namespace FilmManagerCore
{
    public class Logger : IObservable<LogMessage>
    {
        ConcurrentBag<IObserver<LogMessage>> _observers;

        public Logger()
        {
            _observers = new ConcurrentBag<IObserver<LogMessage>>();
        }

        public IDisposable Subscribe(IObserver<LogMessage> observer)
        {
            _observers.Add(observer);
            return new LoggerUnsubscriber(this, observer);
        }

        public void AddMessage(LogMessage message)
        {
            
            foreach (var observer in GetConcreteObservers(message))
                OnObserverNext(observer, message);
        }

        public async Task AddMessageAsync(LogMessage message)
        {
            foreach (var observer in GetConcreteObservers(message))
                await OnObserverNextAsync(observer, message);
        }

        static void OnObserverNext(IObserver<LogMessage> observer, LogMessage message)
        {
            observer.OnNext(message);
        }

        async static Task OnObserverNextAsync(IObserver<LogMessage> observer, LogMessage message)
        {
            await Task.Run(() => OnObserverNext(observer, message));
        }

        internal void Unsubscribe(IObserver<LogMessage> observer)
        {
            _observers.TryTake(out observer);
        }

        /// <summary>
        /// Выбрать наблюдателей типа LogBaseObserver, которые ловят нужный тип лога, или универсальные
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <returns></returns>
        IEnumerable<IObserver<LogMessage>> GetConcreteObservers(LogMessage message)
        {
            //return _observers.Where(o => (o as ILogObserver)?.GetLogTypes()?.Contains(message.Type) ?? true || (o as ILogObserver).GetLogTypes().Count() == 0);
            return _observers.Where(o =>
            ((o as ILogObserver)?.IsSwitchedOn ?? true) &&
            (((o as ILogObserver)?.IsUniversal ?? true) ||
            ((o as ILogObserver).GetLogTypes()?.Contains(message.Type) ?? true) ||
            (o as ILogObserver).GetLogTypes().Count() == 0));
        }
    }
}
