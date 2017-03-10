using LineMetrics.API.Exceptions;
using LineMetrics.API.RequestTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class Template
    {
        internal LineMetricsService ServiceInstance { get; set; }

        private IList<TemplateRequiredFields> requiredFields = null;

        [DataMember(Name = "uid")]
        public string Id { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        public IList<TemplateRequiredFields> RequiredFields
        {
            get
            {
                return requiredFields ?? ( requiredFields = ServiceInstance.LoadTemplateRequiredFields(this));
            }
        }

        public string CreateAsset(Dictionary<string, LineMetrics.API.DataTypes.Base> data)
        {
            if (RequiredFields.Count != data.Count)
            {
                throw new ServiceException("Please provide data for all required fields!");
            }

            CreateAssetRequest req = new CreateAssetRequest();
            req.TemplateId = Id;

            foreach (var kvp in data)
            {
                req.PayLoad.Add(kvp.Key, kvp.Value);
            }

            return ServiceInstance.TemplateService.CreateAsset(ServiceInstance.AuthenticationToken, req);
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, Name: {1}", Id, Name);
        }
    }
}
