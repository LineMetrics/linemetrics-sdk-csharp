using LineMetrics.API.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LineMetrics.API.Extensions;
using LineMetrics.API.Helper;

namespace LineMetrics.API.RequestTypes
{
    [KnownType(typeof(DataTypes.Double))]
    [KnownType(typeof(DoubleAverage))]
    [KnownType(typeof(GeoCoord))]
    [KnownType(typeof(GeoAddress))]
    [KnownType(typeof(Bool))]
    [KnownType(typeof(DataTypes.String))]
    [KnownType(typeof(DataTypes.Timestamp))]
    public class CreateAssetRequest : BaseRequest
    {
        JsonDictionary<string, Base> payLoad;

        public JsonDictionary<string, Base> PayLoad
        {
            get
            {
                return payLoad ?? (payLoad = new JsonDictionary<string, Base>());
            }
        }

        public string TemplateId { get; set; }


        internal override Uri BuildRequestUri(Uri baseUri, string uriPath, string uriPathAppendix = null, bool skipRequestParameters = false)
        {
            uriPath.AssertNotNullOrEmpty("uriPath");

            if (TemplateId.IsNullOrWhiteSpace() )
            {
                throw new ArgumentException("TemplateId must not be null or empty!");
            }
            uriPath += "/" + TemplateId;

            return new Uri(baseUri, uriPath);
        }

        internal override string AppendRequestString()
        {
            throw new NotImplementedException();
        }
    }
}
