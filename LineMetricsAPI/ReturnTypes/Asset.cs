using LineMetrics.API.Extensions;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using LineMetrics.API.RequestTypes;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class Asset : ObjectBase
    {
        [DataMember(Name = "custom_key")]
        public string CustomKey { get; set; }

        [DataMember(Name = "children_info")]
        public Dictionary<string, object> ChildrenInfo { get; set; }

        public string Image
        {
            get
            {
                if (Payload != null && Payload.ContainsKey("image"))
                {
                    return (string)Payload["image"];
                }
                return null;
            }
        }

        public string GetImage()
        {
            return Image;
        }

        /*public List<string> Images
        {
            get
            {
                var images = new List<string>();
                if (Payload != null && Payload.ContainsKey("images"))
                {
                    if (Payload["images"] != null)
                    {
                        foreach (var obj in (object[])Payload["images"])
                        {
                            images.Add((string)obj);
                        }
                    }
                }
                return images;
            }
        }*/

        /// <summary>
        /// Loads all child assets
        /// </summary>
        /// <returns>A list auf all child assets</returns>
        public List<Asset> LoadAssets()
        {
            var assets = ServiceInstance.LoadAssets(Helper.Constants.ResourceTypes.Asset, Id).Cast<Asset>().ToList();

            assets.ForEach(a => a.Parent = this);

            return assets;
        }

        public List<Property> LoadProperties()
        {
            var properties = ServiceInstance.LoadAssets(Helper.Constants.ResourceTypes.Property, Id).Cast<Property>().ToList();

            properties.ForEach(p => p.Parent = this);

            return properties;
        }

        public List<DataStream> LoadDataStreams()
        {
            var streams = ServiceInstance.LoadAssets(Helper.Constants.ResourceTypes.DataStream, Id).Cast<DataStream>().ToList();

            streams.ForEach(s => s.Parent = this);

            return streams;
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, Type: {1}, CustomKey: {2}, TemplateId: {3}, ParentId: {4}, Title: {5}, Icon: {6}, Image: {7}", Id, Type, CustomKey, TemplateId, ParentId, Title, Icon, Image);
        }

        public override string GetId()
        {
            return Id;
        }

        public override string Id
        {
            get
            {
                if (!CustomKey.IsNullOrWhiteSpace())
                {
                    return CustomKey;
                }
                else
                {
                    return ObjectId;
                }
            }
        }

        // TODO save return auf string?
        public override void Save()
        {
            UpdateObjectRequest req = new UpdateObjectRequest
            {
                ObjectId = ObjectId,
                Data = new UpdateObjectRequest.UpdateData
                {
                    CustomKey = CustomKey,
                    Name = Title, //?
                    Parent = ParentId
                }
            };
            ServiceInstance.ObjectService.Update(ServiceInstance.AuthenticationToken, req);
        }
    }
}
