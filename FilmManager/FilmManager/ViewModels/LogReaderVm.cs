using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FilmManagerCore;
using FilmManagerCore.Models;

namespace FilmManager.ViewModels
{
    public class LogReaderVm : Notifier, ILogObserver
    {
        LogMessage _lastMessage;
        bool _isSwitchedOn;

        public ObservableCollection<LogMessage> Messages { get; }

        public bool IsUniversal
        {
            get
            {
                return true;
            }
        }

        public LogReaderVm()
        {
            Messages = new ObservableCollection<LogMessage>();
            IsSwitchedOn = true;
        }

        public LogMessage LastMessage
        {
            get
            {
                return _lastMessage;
            }

            set
            {
                if (_lastMessage != value)
                {
                    _lastMessage = value;
                    OnPropertyChanged(nameof(LastMessage));
                }
            }
        }

        public bool IsSwitchedOn
        {
            get
            {
                return _isSwitchedOn;
            }

            set
            {
                if (_isSwitchedOn != value)
                {
                    _isSwitchedOn = value;
                    OnPropertyChanged(nameof(_isSwitchedOn));
                }
            }
        }

        public IEnumerable<string> GetLogTypes()
        {
            throw new NotImplementedException();
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
            LastMessage = value;
            Messages.Add(value);
        }
    }
}
