using LineMetrics.API.Exceptions;
using LineMetrics.API.Extensions;
using LineMetrics.API.ReturnTypes;
using System;
using System.Net;

namespace LineMetrics.API.Services
{
    public class OAuth2Service : ServiceBase
    {
        public OAuth2Service(LineMetricsService serviceInstance)
            : base(serviceInstance)
        {
        }

        public OAuth2Token Authenticate(string clientId, string clientSecret)
        {
            try
            {
                if (clientId.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("clientId must not be null or empty!");
                }

                if (clientSecret.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("clientId must not be null or empty!");
                }

                Uri uri = new Uri(baseUri, "oauth/access_token");
                using (var client = CreateSSLClient())
                {
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("client_id", clientId);
                    reqparm.Add("grant_type", "client_credentials");
                    reqparm.Add("client_secret", clientSecret);
                    var result = client.UploadValues(uri, reqparm);

                    return ToObject<OAuth2Token>(result);
                }
            }
            catch (WebException e)
            {
                var error = ParseErrorResponse(e);
                if (null != error)
                {
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

        public OAuth2Token Authenticate(string clientId, string clientSecret, string email, string password)
        {
            try
            {
                if (clientId.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("clientId must not be null or empty!");
                }

                if (clientSecret.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("clientSecret must not be null or empty!");
                }

                if (email.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("email must not be null or empty!");
                }

                if (password.IsNullOrWhiteSpace())
                {
                    throw new ArgumentException("password must not be null or empty!");
                }

                Uri uri = new Uri(baseUri, "oauth/access_token");
                using (var client = CreateSSLClient())
                {
                    var reqparm = new System.Collections.Specialized.NameValueCollection();
                    reqparm.Add("client_id", clientId);
                    reqparm.Add("grant_type", "password");
                    reqparm.Add("client_secret", clientSecret);
                    reqparm.Add("email", email);
                    reqparm.Add("password", password);
                    var result = client.UploadValues(uri, reqparm);

                    return ToObject<OAuth2Token>(result);
                }
            }
            catch (WebException e)
            {
                var error = ParseErrorResponse(e);
                if (null != error)
                {
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
