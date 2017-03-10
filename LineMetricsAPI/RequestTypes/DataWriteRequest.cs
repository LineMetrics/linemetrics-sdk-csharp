using LineMetrics.API.DataTypes;
using LineMetrics.API.ReturnTypes;
using System;
using System.Runtime.Serialization;
using LineMetrics.API.Extensions;
using System.Collections.Generic;
using LineMetrics.API.Helper;

namespace LineMetrics.API.RequestTypes
{
    [KnownType(typeof(DataTypes.Double))]
    [KnownType(typeof(DoubleAverage))]
    [KnownType(typeof(GeoCoord))]
    [KnownType(typeof(GeoAddress))]
    [KnownType(typeof(Bool))]
    [KnownType(typeof(DataTypes.String))]
    [KnownType(typeof(DataTypes.Timestamp))]
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
