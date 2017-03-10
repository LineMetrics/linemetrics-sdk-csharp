using LineMetrics.API.Extensions;
using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class AggregatedDataReadResponse : DataReadReponse
    {
        [DataMember(Name = "ts")]
        private long unixTicks;

        [IgnoreDataMember]
        public DateTime Timestamp
        {
            get
            {
                return unixTicks.DateTimeFromUnix();
            }
            private set
            {
                unixTicks = value.UnixTicks();
            }
        }

        [DataMember(Name = "average")]
        public double? Average { get; set; }
        [DataMember(Name = "min")]
        public double? Minimum { get; set; }
        [DataMember(Name = "max")]
        public double? Maximum { get; set; }
        [DataMember(Name = "sum")]
        public double? Sum { get; set; }

        [DataMember(Name = "count")]
        public int? Count { get; set; }

        public override string ToString()
        {
            return string.Format("Average: {0}, Minimum: {1}, Maximum: {2}, Sum: {3}, Count: {4}, Timestamp: {5:dd.MM.yyy HH:mm:ss}", 
                Average, Minimum, Maximum, Sum, Count, Timestamp);
        }
    }
}
