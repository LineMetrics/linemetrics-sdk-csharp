using LineMetrics.API.Services;
using System.Runtime.Serialization;
using LineMetrics.API.Helper;
using System.Collections.Generic;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class Table : Base
    {
        [IgnoreDataMember]
        private JsonDictionary<string, Base> columns;

        [IgnoreDataMember]
        private Dictionary<string, object> jsonDictionaryCache;

        [DataMember(Name = "val")]
        public JsonDictionary<string, Base> Columns
        {
            get { return columns ?? (columns = new JsonDictionary<string, Base>()); }
            set
            {
                columns = value;
            }
        }

        public Table() { }

        internal Table(Dictionary<string, object> columnData)
        {
            jsonDictionaryCache = columnData;
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
                if (Columns.TryGetValue(name, out val))
                {
                    return (T)val;
                }
            }
            return null;
        }

        public override string ToString()
        {
            return string.Format("Value: {0}, Timestamp: {1:dd.MM.yyyy HH:mm:ss}", Columns.Count, Timestamp);
        }
    }
}
