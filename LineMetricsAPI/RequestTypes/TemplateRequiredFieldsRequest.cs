using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.RequestTypes
{
    public class TemplateRequiredFieldsRequest : BaseRequest
    {
        public string TemplateId { get; set; }

        internal override Uri BuildRequestUri(Uri baseUri, string uriPath, string uriPathAppendix = null, bool skipRequestParameters = false)
        {
            uriPath.AssertNotNullOrEmpty("uriPath");

            if (TemplateId.IsNullOrWhiteSpace())
            {
                throw new ArgumentException("Template must not be null!");
            }

            uriPath += "/" + TemplateId;

            if (!uriPathAppendix.IsNullOrWhiteSpace())
            {
                uriPath += "/" + uriPathAppendix;
            }

            return new Uri(baseUri, uriPath);
        }

        internal override string AppendRequestString()
        {
            throw new NotImplementedException();
        }
    }
}
