using LineMetrics.API.DataTypes;
using LineMetrics.API.Helper;
using System.Runtime.Serialization;

namespace LineMetrics.API.RequestTypes
{
    [KnownType(typeof(DataTypes.Double))]
    [KnownType(typeof(DoubleAverage))]
    [KnownType(typeof(GeoCoord))]
    [KnownType(typeof(GeoAddress))]
    [KnownType(typeof(Bool))]
    [KnownType(typeof(DataTypes.String))]
    [KnownType(typeof(DataTypes.Timestamp))]
    [KnownType(typeof(Table))]
    public class KeyValueDataWriteRequest : BaseDataWriteRequest
    {
        JsonDictionary<string, Base> payLoad;

        public JsonDictionary<string, Base> PayLoad
        {
            get
            {
                return payLoad ?? (payLoad = new JsonDictionary<string, Base>());
            }
        }
    }
}
