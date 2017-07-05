using LineMetrics.API.DataTypes;
using System.Runtime.Serialization;
using System.Collections.Generic;

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
    public class DataWriteRequest : BaseDataWriteRequest
    {
        List<Base> payLoad;

        public List<Base> PayLoad
        {
            get
            {
                return payLoad ?? (payLoad = new List<Base>());
            }
        }
    }
}
