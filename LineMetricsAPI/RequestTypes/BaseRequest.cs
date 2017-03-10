using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LineMetrics.API.Extensions;
using System.Runtime.Serialization;

namespace LineMetrics.API.RequestTypes
{
    public abstract class BaseRequest
    {
        // TODO refactor, new layer without customkey/alias

        [DataMember(Name = "custom_key", IsRequired = false, EmitDefaultValue = false)]
        public string CustomKey { get; set; }

        [DataMember(Name = "alias", IsRequired = false, EmitDefaultValue = false)]
        public string Alias { get; set; }

        public string ObjectId { get; set; }

        internal abstract Uri BuildRequestUri(Uri baseUri, string uriPath, string uriPathAppendix = null, bool skipRequestParameters = false);

        internal abstract string AppendRequestString();

        internal string CreateRequestString(params object[] requestParameters)
        {
            if (requestParameters.Length % 2 != 0)
            {
                throw new ArgumentException("Requestparameters have to be in pairs!");
            }

            string result = String.Empty;

            for (int i = 0; i < requestParameters.Length; i += 2)
            {
                if (!requestParameters[i + 1].IsNullOrEmptyString())
                {
                    if (!result.IsNullOrWhiteSpace())
                    {
                        result += "&";
                    }
                    result += requestParameters[i] + "=" + requestParameters[i + 1];
                }
            }

            if (!result.IsNullOrWhiteSpace())
            {
                result = "?" + result;
            }

            return result;
        }
    }
}
