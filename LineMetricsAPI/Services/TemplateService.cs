using LineMetrics.API.Exceptions;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.ReturnTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.Services
{
    public class TemplateService : ServiceBase
    {
        public TemplateService(LineMetricsService serviceInstance)
            : base(serviceInstance)
        {
        }


        public IList<Template> LoadTemplates(OAuth2Token authToken, TemplateRequest templateRequest)
        {
            templateRequest.AssertNotNullOrEmpty("templateRequest");

            try
            {
                Uri uri = templateRequest.BuildRequestUri(baseUri, "v2/templates", null, true);
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);
                    var result = client.DownloadData(uri);

                    var resultList =  ToObject<List<Template>>(result);

                    resultList.ForEach(r => r.ServiceInstance = ServiceInstance);

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

        public IList<TemplateRequiredFields> LoadRequiredFields(OAuth2Token authToken, TemplateRequiredFieldsRequest templateRequiredFieldsRequest)
        {
            templateRequiredFieldsRequest.AssertNotNullOrEmpty("templateRequiredFieldsRequest");

            try
            {
                Uri uri = templateRequiredFieldsRequest.BuildRequestUri(baseUri, "v2/template", "required-fields", true);
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);
                    var result = client.DownloadData(uri);

                    return ToObject<List<TemplateRequiredFields>>(result);
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

        public string CreateAsset(OAuth2Token authToken, CreateAssetRequest createAssetRequest)
        {
            createAssetRequest.AssertNotNullOrEmpty("createAssetRequest");

            try
            {
                Uri uri = createAssetRequest.BuildRequestUri(baseUri, "v2/template", null, true);
                using (var client = CreateSSLClient())
                {
                    SetAuthorizationHeader(client, authToken);
                    SetJsonContentTypeHeader(client);

                    string json = ToJson(createAssetRequest.PayLoad);
                    var response = (Dictionary<string, object>)DeSerialize(client.UploadString(uri, json));

                    return (string)response["uid"];
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
