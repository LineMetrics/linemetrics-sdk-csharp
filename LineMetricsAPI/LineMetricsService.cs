using LineMetrics.API.DataTypes;
using LineMetrics.API.Exceptions;
using LineMetrics.API.Extensions;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.ReturnTypes;
using LineMetrics.API.Services;
using System.Collections.Generic;
using System.Net;

namespace LineMetrics.API
{
    public class LineMetricsService : ILMService
    {
        private readonly OAuth2Service authenticationService;
        private readonly DataService dataService;
        private readonly ObjectService objectService;
        private readonly TemplateService templateService;

        private OAuth2Token authenticationToken;

        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string email;
        private readonly string password;

        public LineMetricsService(string clientId, string clientSecret, IWebProxy proxy, bool useSystemDefaultProxy = false) : this(proxy, useSystemDefaultProxy)
        {
            if (clientId.IsNullOrWhiteSpace())
            {
                throw new ServiceException("clientId must not be null or empty!");
            }

            if (clientSecret.IsNullOrWhiteSpace())
            {
                throw new ServiceException("clientSecret must not be null or empty!");
            }

            this.clientId = clientId;
            this.clientSecret = clientSecret;

            Authenticate(false);
        }

        public LineMetricsService(string clientId, string clientSecret) : this(clientId, clientSecret, null, false) { }

        public LineMetricsService(string clientId, string clientSecret, string email, string password) : this(clientId, clientSecret, email, password, null, false) { }

        public LineMetricsService(string clientId, string clientSecret, string email, string password, IWebProxy proxy, bool useSystemDefaultProxy = false)
            : this(proxy, useSystemDefaultProxy)
        {
            if (clientId.IsNullOrWhiteSpace())
            {
                throw new ServiceException("clientId must not be null or empty!");
            }

            if (clientSecret.IsNullOrWhiteSpace())
            {
                throw new ServiceException("clientSecret must not be null or empty!");
            }

            if (email.IsNullOrWhiteSpace())
            {
                throw new ServiceException("email must not be null or empty!");
            }

            if (password.IsNullOrWhiteSpace())
            {
                throw new ServiceException("password must not be null or empty!");
            }

            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.email = email;
            this.password = password;

            Authenticate(true);
        }

        internal LineMetricsService(IWebProxy proxy, bool useSystemDefaultProxy = false)
        {
            authenticationService = new OAuth2Service(this);
            dataService = new DataService(this);
            objectService = new ObjectService(this);
            templateService = new TemplateService(this);

            SetupProxy(proxy, useSystemDefaultProxy);
        }

        public IWebProxy SetupProxy(IWebProxy proxy, bool useSystemDefaultProxy = false)
        {
            if (useSystemDefaultProxy)
            {
                proxy = WebRequest.GetSystemWebProxy();
                proxy.Credentials = CredentialCache.DefaultCredentials;
            }

            authenticationService.SetProxy(proxy);
            dataService.SetProxy(proxy);
            objectService.SetProxy(proxy);
            templateService.SetProxy(proxy);

            return proxy;
        }

        internal OAuth2Token AuthenticationToken
        {
            get { return authenticationToken; }
        }

        internal ObjectService ObjectService { get { return objectService; } }
        internal DataService DataService { get { return dataService; } }
        internal TemplateService TemplateService { get { return templateService; } }

        internal void Authenticate(bool passwordGrant)
        {
            if (passwordGrant)
            {
                authenticationToken = authenticationService.Authenticate(clientId, clientSecret, email, password);
            }
            else
            {
                authenticationToken = authenticationService.Authenticate(clientId, clientSecret);
            }
        }

        public void Logout()
        {
            authenticationToken = null;
        }

        internal IList<DataWriteResponse> WriteData(DataWriteRequest data)
        {
            return dataService.Write(authenticationToken, data);
        }

        internal IDictionary<string, DataWriteResponse> WriteData(KeyValueDataWriteRequest data)
        {
            return dataService.Write(authenticationToken, data);
        }

        internal IList<DataReadReponse> ReadData(DataReadRequest data)
        {
            return dataService.Read(authenticationToken, data);
        }

        internal Base ReadLastValue(LastValueDataReadRequest data)
        {
            return dataService.ReadLastValue(authenticationToken, data);
        }

        public ObjectBase LoadObject(string id)
        {
            return objectService.LoadObject(authenticationToken, new AssetRequest { CustomKey = id });
        }

        public ObjectBase LoadObject(string id, string alias)
        {
            return objectService.LoadObject(authenticationToken, new AssetRequest { CustomKey = id, Alias = alias });
        }

        public ObjectBase LoadObjectByCustomKey(string customKey)
        {
            return LoadObject(customKey);
        }

        public IList<ObjectBase> LoadAssets(string type, string parentId)
        {
            return objectService.LoadObjects(authenticationToken, new AssetRequest { ObjectType = type, CustomKey = parentId });
        }

        public IList<Asset> LoadAssets()
        {
            return objectService.LoadRootAssets(authenticationToken, new AssetRequest());
        }

        public void CheckToken()
        {
            throw new System.NotImplementedException();
        }

        internal DataStreamType LoadDataStreamConfig(DataStreamConfigRequest req)
        {
            return dataService.LoadDataStreamConfig(authenticationToken, req);
        }

        public IList<Template> LoadTemplates()
        {
            return templateService.LoadTemplates(authenticationToken, new TemplateRequest());
        }

        internal IList<TemplateRequiredFields> LoadTemplateRequiredFields(Template template){
            return templateService.LoadRequiredFields(authenticationToken, new TemplateRequiredFieldsRequest { TemplateId = template.Id });
        }

        public string DeleteObject(ObjectBase obj, bool recursive)
        {
            DeleteObjectRequest req = new DeleteObjectRequest
            {
                CustomKey = obj.ObjectId,
                Recursive = recursive
            };
            return objectService.Delete(authenticationToken, req);
        }
    }
}
