using LineMetrics.API.DataTypes;
using LineMetrics.API.Exceptions;
using LineMetrics.API.Extensions;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.ReturnTypes;
using System;
using System.Collections.Generic;
using System.Net;

namespace LineMetrics.API.Services
{
    public class DataService : ServiceBase
    {
        public DataService(LineMetricsService serviceInstance)
            : base(serviceInstance)
        {
        }

        public DataStreamType LoadDataStreamConfig(OAuth2Token authToken, DataStreamConfigRequest dataStreamConfigRequest)
        {
            dataStreamConfigRequest.AssertNotNullOrEmpty("dataReadRequest");

            try
            {
                Uri uri = dataStreamConfigRequest.BuildRequestUri(baseUri, "v2/data", "config", true);
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);
                    var result = client.DownloadData(uri);

                    return ToObject<DataStreamType>(result);
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

        public IDictionary<string, DataWriteResponse> Write(OAuth2Token authToken, KeyValueDataWriteRequest keyValueDataWriteRequest)
        {
            keyValueDataWriteRequest.AssertNotNullOrEmpty("keyValueDataWriteRequest");

            try
            {
                Uri uri = keyValueDataWriteRequest.BuildRequestUri(baseUri, "v2/data");
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    string json = ToJson(keyValueDataWriteRequest.PayLoad);

                    var response = client.UploadString(uri, json);
                    return ToObject<Dictionary<string,DataWriteResponse>>(response);
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

        public IList<DataWriteResponse> Write(OAuth2Token authToken, DataWriteRequest dataWriteRequest)
        {
            dataWriteRequest.AssertNotNullOrEmpty("dataWriteRequest");

            try
            {
                Uri uri = dataWriteRequest.BuildRequestUri(baseUri, "v2/data");
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    string json = ToJson(dataWriteRequest.PayLoad);

                    var response = client.UploadString(uri, json);
                    return ToObject<List<DataWriteResponse>>(response);
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

        public Base ReadLastValue(OAuth2Token authToken, LastValueDataReadRequest lastValueDataReadRequest)
        {
            lastValueDataReadRequest.AssertNotNullOrEmpty("lastValueDataReadRequest");
            lastValueDataReadRequest.Function = Function.LAST_VALUE;

            try
            {
                Uri uri = lastValueDataReadRequest.BuildRequestUri(baseUri, "v2/data");
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    var result = client.DownloadString(uri);

                    return ToObjectList(result, lastValueDataReadRequest.DataType).FirstOrDefault<Base>();
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

        public IList<DataReadReponse> Read(OAuth2Token authToken, DataReadRequest dataReadRequest)
        {
            dataReadRequest.AssertNotNullOrEmpty("dataReadRequest");

            try
            {
                Uri uri = dataReadRequest.BuildRequestUri(baseUri, "v2/data");
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    string result = client.DownloadString(uri);

                    List<DataReadReponse> results = new List<DataReadReponse>();
                    switch (dataReadRequest.Function)
                    {
                        case Function.RAW:
                            foreach (var item in ToObjectList(result, dataReadRequest.DataType))
                            {
                                results.Add(new RawDataReadResponse(item as Base));
                            }
                            break;
                        default:
                            ToObject<List<AggregatedDataReadResponse>>(result).ForEach(r => results.Add(r));
                            break;
                    }
                    return results;
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
