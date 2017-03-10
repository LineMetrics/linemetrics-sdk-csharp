using LineMetrics.API.DataTypes;
using LineMetrics.API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LineMetrics.API.RequestTypes
{
    [KnownType(typeof(DataTypes.Double))]
    [KnownType(typeof(DoubleAverage))]
    [KnownType(typeof(GeoCoord))]
    [KnownType(typeof(GeoAddress))]
    [KnownType(typeof(Bool))]
    [KnownType(typeof(DataTypes.String))]
    [KnownType(typeof(DataTypes.Timestamp))]
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
