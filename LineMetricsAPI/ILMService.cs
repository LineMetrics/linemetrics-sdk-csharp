using LineMetrics.API.DataTypes;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.ReturnTypes;
using LineMetrics.API.Services;
using System.Collections.Generic;
using System.Net;

namespace LineMetrics.API
{
    public interface ILMService
    {
        ObjectBase LoadObject(string id);
        ObjectBase LoadObject(string id, string alias);
        ObjectBase LoadObjectByCustomKey(string customKey);
        IList<Asset> LoadAssets();
        IList<ObjectBase> LoadAssets(string type, string parentId);
        IList<Template> LoadTemplates();
        string DeleteObject(ObjectBase obj, bool recursive);
        void Logout();
        IWebProxy SetupProxy(IWebProxy proxy, bool useSystemDefaultProxy = false);

        void CheckToken();
    }
}
