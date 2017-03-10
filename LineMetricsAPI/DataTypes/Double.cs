using System.Runtime.Serialization;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class Double : Base
    {
        [DataMember(Name = "val")]
        public double? Value { get; set; }

        public override string ToString()
        {
            return string.Format("Value: {0}, Timestamp: {1:dd.MM.yyy HH:mm:ss}", Value, Timestamp);
        }
    }
}
