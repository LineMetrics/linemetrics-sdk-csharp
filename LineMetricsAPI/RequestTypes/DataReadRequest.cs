using LineMetrics.API.DataTypes;
using LineMetrics.API.Extensions;
using LineMetrics.API.ReturnTypes;
using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.RequestTypes
{
    public class DataReadRequest : BaseDataReadRequest
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public string TimeZone { get; set; }
        public string Granularity { get; set; }


        public DataReadRequest() { }

        public DataReadRequest(Function function, DateTime from, DateTime to, string timeZone = null, string granularity = null)
        {
            Function = function;
            From = from;
            To = to;
            TimeZone = timeZone;
            Granularity = granularity;
        }

        internal override string AppendRequestString()
        {
            return CreateRequestString("function", Function.ToString().ToLowerInvariant(), "time_from", From.UnixTicks(), "time_to", To.UnixTicks(), "time_zone", TimeZone, "granularity", Granularity);
        }
    }
}
