using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace FilmManagerCore.Serializers
{
    class DbToAppModelsConverter
    {
        public static TDest ConvertFromDb<TSource, TDest>(TSource src) where TDest : new()
        {
            var serializer = new XmlSerializer(typeof(TSource));
            var res = default(TDest);
            using (var ms = new MemoryStream())
            {
                serializer.Serialize(ms, src);
                ms.Position = 0;
                var deserializer = new XmlSerializer(typeof(TDest));
                res = (TDest)deserializer.Deserialize(ms);
            }
            return res;
        }
    }
}
