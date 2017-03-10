using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.RequestTypes
{
    public abstract class BaseDataReadRequest : BaseRequest
    {
        public Function Function { get; set; }

        public Type DataType { get; set; }

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
    }
}
