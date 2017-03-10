using LineMetrics.API.Extensions;
using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class Timestamp : Base
    {
        [DataMember(Name = "val", IsRequired = false, EmitDefaultValue = false)]
        private long? val;

        [IgnoreDataMember]
        public DateTime? Value
        {
            get
            {
                if (val.HasValue)
                {
                    return val.Value.DateTimeFromUnix();
                }
                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    val = value.Value.UnixTicks();
                }
                else
                {
                    val = null;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Value: {0:dd.MM.yyyy HH:mm:ss}, Timestamp: {1:dd.MM.yyy HH:mm:ss}", Value, Timestamp);
        }
    }
}
