using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class GeoAddress : GeoCoord
    {
        [DataMember(Name = "val")]
        public string Address { get; set; }

    }
}
