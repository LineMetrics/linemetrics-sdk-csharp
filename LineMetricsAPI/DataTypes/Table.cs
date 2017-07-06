using LineMetrics.API.Services;
using System.Runtime.Serialization;
using LineMetrics.API.Helper;
using System.Collections.Generic;
using System.Linq;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class Table : Base
    {
        // for writing data
        [DataMember(Name = "val")]
        private JsonDictionary<string, Base> columns = new JsonDictionary<string, Base>();

        // for reading data
        [IgnoreDataMember]
        private Dictionary<string, object> jsonDictionaryCache;
        
        public Table() { }

        internal Table(Dictionary<string, object> columnData)
        {
            jsonDictionaryCache = columnData;
        }

        public IList<string> ColumnNames
        {
            get
            {
                if (jsonDictionaryCache != null)
                {
                    return jsonDictionaryCache.Keys.ToList();
                }
                else
                {
                    return columns.Keys;
                }
            }
        }

        public void AddColumn(string name, Base data)
        {
            columns.Add(name, data);
        }

        public T GetColumn<T>(string name) where T : Base
        {
            if (jsonDictionaryCache != null)
            {
                object data;
                if (jsonDictionaryCache.TryGetValue(name, out data))
                {
                    return (T)ServiceBase.LoadObjectFromDictionary((Dictionary<string, object>)data, typeof(T));
                }
            }
            else
            {
                Base val;
                if (columns.TryGetValue(name, out val))
                {
                    return (T)val;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("Value: {0}, Timestamp: {1:dd.MM.yyyy HH:mm:ss}", columns.Count, Timestamp);
        }
    }
}
