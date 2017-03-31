using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FilmManagerCore.Parsing
{
    public class ParsersCollection : ICollection<Parser>
    {
        IList<Parser> _items;

        public ParsersCollection()
        {
            _items = new List<Parser>();
        }

        public ParsersCollection(IEnumerable<Parser> parsers) : this()
        {
            foreach (var parser in parsers)
                Add(parser);
        }

        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return _items.IsReadOnly;
            }
        }

        public void Add(Parser item)
        {
            if (!CheckUrl(item.BaseUrl))
                throw new InvalidOperationException("ParsersCollection.Add: Неверный формат URI или парсер для данного сайта уже существует");
            _items.Add(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public bool Contains(Parser item)
        {
            return _items.Contains(item);
        }

        public void CopyTo(Parser[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }

        public IEnumerator<Parser> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool Remove(Parser item)
        {
            return _items.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        bool CheckUrl(string url)
        {
            Uri uri = null;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate($"http://{url}", UriKind.Absolute, out uri))
                return !_items.Any(p => new Uri(p.BaseUrl).Host.Equals(uri.Host, StringComparison.CurrentCultureIgnoreCase));
            return false;
        }

        public Parser GetParserByUrl(string url)
        {
            Uri uri = null;
            if (Uri.TryCreate(url, UriKind.Absolute, out uri) || Uri.TryCreate($"http://{url}", UriKind.Absolute, out uri))
                return _items.FirstOrDefault(p => new Uri(p.BaseUrl).Host.Equals(uri.Host, StringComparison.CurrentCultureIgnoreCase));
            return null;
        }

        public void Save(string fileName)
        {
            var serializer = new XmlSerializer(typeof(List<Parser>));
            using(var stream = new FileStream(fileName, FileMode.Create))
            {
                serializer.Serialize(stream, _items);
            }
        }

        public void Load(string fileName)
        {
            var serializer = new XmlSerializer(typeof(List<Parser>));
            using(var stream = new FileStream(fileName, FileMode.Open))
            {
                _items = (List<Parser>)serializer.Deserialize(stream);
            }
        }
    }
}
