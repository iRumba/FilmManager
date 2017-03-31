using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FilmManagerCore.Parsing;

namespace FilmManager.ViewModels
{
    public class ParsersVm
    {
        public static RoutedCommand AddParserCommand = new RoutedCommand();
        public static RoutedCommand RemoveParserCommand = new RoutedCommand();
        public static RoutedCommand AddFilmListCommand = new RoutedCommand();
        public static RoutedCommand RemoveFilmListCommand = new RoutedCommand();

        public ObservableCollection<Parser> Parsers { get; }

        public ParsersVm(IEnumerable<Parser> parsers)
        {
            Parsers = new ObservableCollection<Parser>(parsers);
        }
    }
}
