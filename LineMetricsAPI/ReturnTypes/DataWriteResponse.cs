using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class DataWriteResponse
    {
        [DataMember(Name = "response")]
        private string ResponseHelper
        {
            get
            {
                return Response.ToString();
            }
            set
            {
                Response = (Response)Enum.Parse(typeof(Response), value, true);
            }
        }

        [IgnoreDataMember]
        public Response Response { get; set; }

        public override string ToString()
        {
            return String.Format("Response: {0}", Response);
        }
    }
}
