using LineMetrics.API.Exceptions;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.ReturnTypes;
using System;
using System.Collections.Generic;
using System.Net;
using LineMetrics.API.Extensions;
using System.Linq;
using System.Collections.Specialized;

namespace LineMetrics.API.Services
{
    public class ObjectService : ServiceBase
    {
        public ObjectService(LineMetricsService serviceInstance)
            : base(serviceInstance)
        {
        }

        private Type GetTargetType(string objectType)
        {
            Type targetType = null;
            switch (objectType)
            {
                case Helper.Constants.ResourceTypes.DataStream:
                    targetType = typeof(DataStream);
                    break;
                case Helper.Constants.ResourceTypes.Property:
                    targetType = typeof(Property);
                    break;
                case Helper.Constants.ResourceTypes.Asset:
                default:
                    targetType = typeof(Asset);
                    break;
            }
            return targetType;
        }

        internal ObjectBase LoadObject(OAuth2Token authToken, AssetRequest assetRequest)
        {
            assetRequest.AssertNotNullOrEmpty("assetRequest");

            try
            {
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    Uri uri = assetRequest.BuildRequestUri(baseUri, "v2/object", null);
                    var result = client.DownloadString(uri);

                    var deserializedResult = (Dictionary<string, object>)DeSerialize(result);

                    var objectType = deserializedResult["object_type"].ToString();
                    Type targetType = GetTargetType(objectType);
                    var assetBase = (ObjectBase)LoadObjectFromDictionary(deserializedResult, targetType);
                    assetBase.ServiceInstance = ServiceInstance;

                    return assetBase;
                }
            }
            catch (WebException e)
            {
                var error = ParseErrorResponse(e);
                if (null != error)
                {
                    if (error.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new AuthorizationException(error);
                    }
                    throw new RequestException(error);
                }
                else
                {
                    throw new ServiceException(e.Message);
                }
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }

        internal IList<Asset> LoadRootAssets(OAuth2Token authToken, AssetRequest assetRequest)
        {
            assetRequest.AssertNotNullOrEmpty("assetRequest");

            assetRequest.ObjectType = Helper.Constants.ResourceTypes.Asset;
            assetRequest.CustomKey = null;
            assetRequest.Alias = null;

            return LoadObjects(authToken, assetRequest).Cast<Asset>().ToList();
        }

        internal IList<ObjectBase> LoadObjects(OAuth2Token authToken, AssetRequest assetRequest)
        {
            assetRequest.AssertNotNullOrEmpty("assetRequest");

            try
            {
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);
                    Uri uri = assetRequest.BuildRequestUri(baseUri, "v2/children", null);
                    var result = client.DownloadString(uri);

                    var deserializedResult = DeSerialize(result);
                    var resultList = new List<ObjectBase>();
                    foreach (Dictionary<string, object> obj in (object[])deserializedResult)
                    {
                        var objectType = obj["object_type"].ToString();
                        Type targetType = GetTargetType(objectType);
                        var assetBase = (ObjectBase)LoadObjectFromDictionary(obj, targetType);
                        assetBase.ServiceInstance = ServiceInstance;
                        resultList.Add(assetBase);
                    }
                    return resultList;
                }
            }
            catch (WebException e)
            {
                var error = ParseErrorResponse(e);
                if (null != error)
                {
                    if (error.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new AuthorizationException(error);
                    }
                    throw new RequestException(error);
                }
                else
                {
                    throw new ServiceException(e.Message);
                }
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }

        internal string Update(OAuth2Token authToken, UpdateObjectRequest updateRequest)
        {
            updateRequest.AssertNotNullOrEmpty("updateRequest");

            try
            {
                Uri uri = updateRequest.BuildRequestUri(baseUri, "v2/object", null, true);
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    string json = ToJson(updateRequest.Data);

                    var response = (Dictionary<string, object>)DeSerialize(client.UploadString(uri, json));

                    return (string)response["message"];
                }
            }
            catch (WebException e)
            {
                var error = ParseErrorResponse(e);
                if (null != error)
                {
                    if (error.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new AuthorizationException(error);
                    }
                    throw new RequestException(error);
                }
                else
                {
                    throw new ServiceException(e.Message);
                }
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }

        internal string Delete(OAuth2Token authToken, DeleteObjectRequest deleteRequest)
        {
            deleteRequest.AssertNotNullOrEmpty("deleteRequest");

            try
            {
                Uri uri = deleteRequest.BuildRequestUri(baseUri, "v2/object");
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);

                    var response = (Dictionary<string, object>)DeSerialize(client.UploadString(uri, "DELETE", String.Empty));

                    return (string)response["message"];
                }
            }
            catch (WebException e)
            {
                var error = ParseErrorResponse(e);
                if (null != error)
                {
                    if (error.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new AuthorizationException(error);
                    }
                    throw new RequestException(error);
                }
                else
                {
                    throw new ServiceException(e.Message);
                }
            }
            catch (Exception e)
            {
                throw new ServiceException(e.Message);
            }
        }
    }
}
