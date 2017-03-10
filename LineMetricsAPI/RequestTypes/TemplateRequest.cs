using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.RequestTypes
{
    public class TemplateRequest : BaseRequest
    {
        internal override Uri BuildRequestUri(Uri baseUri, string uriPath, string uriPathAppendix = null, bool skipRequestParameters = false)
        {
            uriPath.AssertNotNullOrEmpty("uriPath");

            return new Uri(baseUri, uriPath);
        }

        internal override string AppendRequestString()
        {
            throw new NotImplementedException();
        }
    }
}
