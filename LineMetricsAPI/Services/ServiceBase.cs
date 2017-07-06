using LineMetrics.API.DataTypes;
using LineMetrics.API.Exceptions;
using LineMetrics.API.ReturnTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web.Script.Serialization;
using LineMetrics.API.Extensions;

namespace LineMetrics.API.Services
{
    public abstract class ServiceBase
    {
        public ServiceBase(LineMetricsService serviceInstance)
        {
            ServiceInstance = serviceInstance;
        }

        internal readonly LineMetricsService ServiceInstance;

        internal static readonly Uri baseUri = new Uri("https://lm3api.linemetrics.com");

        private IWebProxy proxy = null;

        internal WebClient CreateSSLClient()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(AcceptAllCertifications);
            WebClient client = new WebClient();
            client.BaseAddress = baseUri.ToString();

            if (proxy != null)
            {
                client.Proxy = proxy;
            }
            
            var buildVersion = Assembly.GetAssembly(typeof(ServiceBase)).GetName().Version;
            
            client.Headers.Add("user-agent", "C# client library - version: " + buildVersion);
            client.Encoding = Encoding.UTF8;
            return client;
        }

        internal void SetProxy(IWebProxy proxy)
        {
            this.proxy = proxy;
        }

        private static bool AcceptAllCertifications(object sender,
                                           System.Security.Cryptography.X509Certificates.X509Certificate certification,
                                           System.Security.Cryptography.X509Certificates.X509Chain chain,
                                           System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        internal static string Serialize(object obj)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            return serializer.Serialize(obj);
        }

        internal static object DeSerialize(string json)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.DeserializeObject(json);
        }

        internal static T LoadObjectFromDictionary<T>(Dictionary<string, object> data) where T : ObjectBase
        {
            return LoadObjectFromDictionary(data, typeof(T)) as T;
        }

        internal static object LoadObjectFromDictionary(Dictionary<string, object> data, Type targetType)
        {
            // TODO check if type is assetbase?

            var instance = Activator.CreateInstance(targetType);

            foreach (var propInfo in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                DataMemberAttribute attr = (DataMemberAttribute)propInfo.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault();

                if (attr != null && data.ContainsKey(attr.Name))
                {
                    var val = data[attr.Name];
                    var propType = propInfo.PropertyType.IsNullableType() ? Nullable.GetUnderlyingType(propInfo.PropertyType) : propInfo.PropertyType;
                    var convertedValue = Convert.ChangeType(val, propType);

                    propInfo.SetValue(instance, convertedValue, null);
                }
            }

            foreach (var fieldInfo in targetType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance))
            {
                DataMemberAttribute attr = (DataMemberAttribute)fieldInfo.GetCustomAttributes(typeof(DataMemberAttribute), false).FirstOrDefault();

                if (attr != null && data.ContainsKey(attr.Name))
                {
                    var val = data[attr.Name];
                    var propType = fieldInfo.FieldType.IsNullableType() ? Nullable.GetUnderlyingType(fieldInfo.FieldType) : fieldInfo.FieldType;
                    var convertedValue = Convert.ChangeType(val, propType);

                    fieldInfo.SetValue(instance, convertedValue);
                }
            }

            return instance;
        }

        internal void SetAuthorizationHeader(WebClient client, OAuth2Token token)
        {
            if (null == token)
            {
                throw new ServiceException("AuthorizationToken must not be null!");
            }
            client.Headers["Authorization"] = string.Format("{0} {1}", token.TokenType, token.AccessToken);
        }

        internal void SetJsonContentTypeHeader(WebClient client)
        {
            client.Headers[HttpRequestHeader.ContentType] = "application/json";
        }

        internal string ToJson<T>(T value)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.WriteObject(stream, value);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        internal IList ToObjectList(string json, Type type)
        {

            if (type == typeof(Table))
            {
                IList result = new List<Table>();
                // replace the __type attributes of the json response to prevent a deserialization error
                var data = (IList)DeSerialize(json.Replace("\"__type\":\"", "\"type\":\""));
                foreach(Dictionary<string, object> tableData in data)
                {
                    Table table = new Table((Dictionary<string, object>)tableData["val"]);
                    table.unixTicks = (long)tableData["ts"];
                    result.Add(table);
                }
                return result;
            }

            var serializer = new DataContractJsonSerializer(typeof(List<>).MakeGenericType(type));
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (IList)serializer.ReadObject(stream);
            }
        }

        internal LineMetrics.API.DataTypes.Base ToObject(string json, Type type)
        {
            var serializer = new DataContractJsonSerializer(type);
            using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                return (LineMetrics.API.DataTypes.Base)serializer.ReadObject(stream);
            }
        }

        internal T ToObject<T>(string json)
        {
            return ToObject<T>(Encoding.UTF8.GetBytes(json));
        }

        internal T ToObject<T>(byte[] json)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            using (var stream = new MemoryStream(json))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        internal ErrorResponse ParseErrorResponse(WebException e)
        {
            var serializer = new DataContractJsonSerializer(typeof(ErrorResponse));
            using (var responseStream = e.Response.GetResponseStream())
            {
                var error = (ErrorResponse)serializer.ReadObject(responseStream);
                HttpWebResponse response = e.Response as HttpWebResponse;
                if (null != response)
                {
                    error.StatusCode = response.StatusCode;
                    error.StatusDescription = response.StatusDescription;
                }

                return error;
            }
        }
    }
}
