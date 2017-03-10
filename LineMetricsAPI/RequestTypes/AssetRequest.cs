using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.RequestTypes
{
    [DataContract]
    internal class AssetRequest : BaseRequest
    {
        [DataMember(Name = "limit", IsRequired = false, EmitDefaultValue = false)]
        public int? Limit { get; set; }

        [DataMember(Name = "offset", IsRequired = false, EmitDefaultValue = false)]
        public int? Offset { get; set; }

        // TODO make list of strings?
        [DataMember(Name = "object_type", IsRequired = false, EmitDefaultValue = false)]
        public string ObjectType { get; set; }

        internal override Uri BuildRequestUri(Uri baseUri, string uriPath, string uriPathAppendix = null, bool skipRequestParameters = false)
        {
            uriPath.AssertNotNullOrEmpty("uriPath");
            if (!ObjectId.IsNullOrWhiteSpace())
            {
                uriPath += "/" + ObjectId;
            }
            else
            {
                if (CustomKey.IsNullOrWhiteSpace() && !Alias.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("No CustomKey given, can not resolve Alias!");
                }

                // TODO check if last char is a / (and trim?)

                if (!CustomKey.IsNullOrWhiteSpace())
                {
                    uriPath += "/" + CustomKey;
                }

                if (!Alias.IsNullOrWhiteSpace())
                {
                    uriPath += "/" + Alias;
                }
            }

            if (!skipRequestParameters)
            {
                uriPath += AppendRequestString();
            }

            return new Uri(baseUri, uriPath);
        }

        internal override string AppendRequestString()
        {
            return CreateRequestString("limit", Limit, "offset", Offset, "object_type", ObjectType);
        }
    }
}
