using LineMetrics.API.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.RequestTypes
{
    [KnownType(typeof(DataTypes.Double))]
    [KnownType(typeof(DoubleAverage))]
    [KnownType(typeof(GeoCoord))]
    [KnownType(typeof(GeoAddress))]
    [KnownType(typeof(Bool))]
    [KnownType(typeof(DataTypes.String))]
    [KnownType(typeof(DataTypes.Timestamp))]
    public abstract class   BaseDataWriteRequest : BaseRequest
    {
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

            return new Uri(baseUri, uriPath);
        }

        internal override string AppendRequestString()
        {
            throw new NotImplementedException();
        }
    }
}
