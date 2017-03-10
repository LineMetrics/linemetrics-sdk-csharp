using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LineMetrics.API.ReturnTypes
{
    public abstract class ObjectBase
    {
        private Dictionary<string, object> payload;

        private Asset parent;
        internal LineMetricsService ServiceInstance { get; set; }

        [DataMember(Name = "object_id")]
        public string ObjectId { get; set; }

        [DataMember(Name = "object_type")]
        public string Type { get; set; }

        [DataMember(Name = "template_id")]
        public string TemplateId { get; set; }

        [DataMember(Name = "parent_id")]
        public string ParentId { get; set; }

        [DataMember(Name = "payload")]
        public Dictionary<string, object> Payload
        {
            get
            {
                return payload ?? (payload = new Dictionary<string,object>());
            }
            set
            {
                payload = value;
            }
        }

        [IgnoreDataMember]
        public Asset Parent
        {
            get
            {
                if (parent == null || parent.ObjectId != ParentId)
                {
                    parent = (Asset)ServiceInstance.LoadObject(ParentId);
                }

                return parent;
            }
            set
            {
                parent = value;
            }
        }
        
        public abstract string GetId();
        public abstract string Id { get; }
        public abstract void Save();

        public string Title
        {
            get
            {
                if (Payload != null && Payload.ContainsKey("title"))
                {
                    return (string)Payload["title"];
                }
                return null;
            }
            set
            {
                // TODO lazy intialize palyoad dictionary??
                if (Payload != null)
                {
                    if (Payload.ContainsKey("title"))
                    {
                        Payload["title"] = value;
                    }
                    else
                    {
                        Payload.Add("title", value);
                    }
                }
            }
        }

        public string GetTitle()
        {
            return Title;
        }

        public string Icon
        {
            get
            {
                if (Payload != null && Payload.ContainsKey("icon"))
                {
                    return (string)Payload["icon"];
                }
                return null;
            }
            set
            {
                if (Payload != null)
                {
                    if (Payload.ContainsKey("icon"))
                    {
                        Payload["icon"] = value;
                    }
                    else
                    {
                        Payload.Add("icon", value);
                    }
                }
            }
        }

        public string GetIcon()
        {
            return Icon;
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, Type: {1}, TemplateId: {2}, ParentId: {3}, Title: {4}, Icon: {5}", Id, Type, TemplateId, ParentId, Title, Icon);
        }
    }
}
