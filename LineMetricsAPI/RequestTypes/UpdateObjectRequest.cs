using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineMetrics.API.Extensions;
using System.Runtime.Serialization;

namespace LineMetrics.API.RequestTypes
{
    public class UpdateObjectRequest : BaseRequest
    {
        [DataContract]
        public class UpdateData
        {
            [DataMember(Name = "custom_key", IsRequired = false, EmitDefaultValue = false)]
            public string CustomKey { get; set; }

            [DataMember(Name = "alias", IsRequired = false, EmitDefaultValue = false)]
            public string Alias { get; set; }

            [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
            public string Name { get; set; }

            [DataMember(Name = "parent", IsRequired = false, EmitDefaultValue = false)]
            public string Parent { get; set; }
        }

        public UpdateData Data { get; set; }

        internal override Uri BuildRequestUri(Uri baseUri, string uriPath, string uriPathAppendix = null, bool skipRequestParameters = false)
        {
            uriPath.AssertNotNullOrEmpty("uriPath");

            if (ObjectId.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("ObjectId must not be null!");
            }
            uriPath += "/" + ObjectId;

            return new Uri(baseUri, uriPath);
        }

        internal override string AppendRequestString()
        {
            throw new NotImplementedException();
        }
    }
}
