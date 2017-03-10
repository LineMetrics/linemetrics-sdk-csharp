using System;
using System.Collections.Generic;
using System.Net;
using System.Runtime.Serialization;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class ErrorResponse
    {
        [DataMember(Name = "message")]
        public string Message { get; set; }

        [DataMember(Name = "erros")]
        private Dictionary<string, object> Errors { get; set; }

        [IgnoreDataMember]
        public HttpStatusCode StatusCode { get; set; }

        [IgnoreDataMember]
        public string StatusDescription { get; set; }

        public override string ToString()
        {
            return String.Format("StatusCode: {0} ({1}), Message: {2}", StatusCode, (int)StatusCode, Message);
        }
    }
}
