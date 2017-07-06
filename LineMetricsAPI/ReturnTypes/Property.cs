using LineMetrics.API.Exceptions;
using LineMetrics.API.Extensions;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.Services;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class Property : ObjectBase
    {
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "data")]
        private Dictionary<string, object> dataDictionary;

        private LineMetrics.API.DataTypes.Base _value; 

        [IgnoreDataMember]
        public LineMetrics.API.DataTypes.Base Value
        {
            get
            {
                if (dataDictionary == null)
                {
                    LastValueDataReadRequest req = new LastValueDataReadRequest
                    {
                        DataType = DataTypes.Output,
                        ObjectId = ObjectId
                    };
                    _value = ServiceInstance.ReadLastValue(req);
                }
                else
                {
                    if (_value == null)
                    {
                        _value = (LineMetrics.API.DataTypes.Base)ServiceBase.LoadObjectFromDictionary(dataDictionary, DataTypes.Output);
                    }
                }
                return _value;
            }
            set
            {
                if (value.GetType() != DataTypes.Input)
                {
                    throw new ServiceException("Input type '{0}' was expected, given '{1}'!", DataTypes.Input.Name, value.GetType().Name);
                }
                _value = value;
                DataWriteRequest req = new DataWriteRequest
                {
                    ObjectId = ObjectId
                };
                req.PayLoad.Add(_value);
                ServiceInstance.WriteData(req);
            }
        }

        private DataStreamType dataTypes;

        [IgnoreDataMember]
        public DataStreamType DataTypes
        {
            get
            {
                DataStreamConfigRequest req = new DataStreamConfigRequest
                {
                    CustomKey = ParentId,
                    Alias = Id,
                    ObjectId = ObjectId + "/" //fix von Klemens -> wenn ObjectId angegeben ist dann muss der Rest-Call so ausschauen: [ObjectId]//config
                };
                return dataTypes ?? (dataTypes = ServiceInstance.LoadDataStreamConfig(req));
            }
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(", Alias: {0}, Data: {1}", Alias, Value);
        }

        public override string GetId()
        {
            return Id;
        }

        public override string Id
        {
            get
            {
                if (!Alias.IsNullOrWhiteSpace()) { return Alias; }
                else
                {
                    return ObjectId;
                }
            }
        }

        public override void Save()
        {
            UpdateObjectRequest req = new UpdateObjectRequest
            {
                ObjectId = ObjectId,
                Data = new UpdateObjectRequest.UpdateData
                {
                    Alias = Alias,
                    Name = Title, //?
                    Parent = ParentId
                }
            };
            ServiceInstance.ObjectService.Update(ServiceInstance.AuthenticationToken, req);
        }
    }
}
