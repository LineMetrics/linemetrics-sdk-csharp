using LineMetrics.API.Extensions;
using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    [KnownType(typeof(DataTypes.Double))]
    [KnownType(typeof(DoubleAverage))]
    [KnownType(typeof(GeoCoord))]
    [KnownType(typeof(GeoAddress))]
    [KnownType(typeof(Bool))]
    [KnownType(typeof(DataTypes.String))]
    [KnownType(typeof(DataTypes.Timestamp))]
    [KnownType(typeof(Table))]
    public abstract class Base
    {
        [DataMember(Name = "ts", IsRequired = false, EmitDefaultValue = false)]
        internal long? unixTicks;

        [IgnoreDataMember]
        public DateTime? Timestamp
        {
            get
            {
                if (unixTicks.HasValue)
                {
                    return unixTicks.Value.DateTimeFromUnix();
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    unixTicks = value.Value.UnixTicks();
                }
                else
                {
                    unixTicks = null;
                }
            }
        }
    }
}
