using System.Runtime.Serialization;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class DoubleAverage : Base
    {
        [DataMember(Name = "val")]
        public double? Value { get; set; }

        [DataMember(Name = "min")]
        public double? Minimum { get; set; }

        [DataMember(Name = "max")]
        public double? Maximum { get; set; }

        public override string ToString()
        {
            return string.Format("Value: {0}, Minimum: {1}, Maximum: {2}, Timestamp: {3:dd.MM.yyy HH:mm:ss}", Value, Minimum, Maximum, Timestamp);
        }
    }
}
