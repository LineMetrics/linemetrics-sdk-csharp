using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using LineMetrics.API.Extensions;
using LineMetrics.API.RequestTypes;
using LineMetrics.API.Exceptions;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class DataStream : ObjectBase
    {
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "input")]
        public string InputId { get; set; }

        //[DataMember(Name = "data_type")]
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

        public IList<DataReadReponse> LoadData(DateTime from, DateTime to, string timeZone, string granularity, Function function = Function.RAW)
        {
            DataReadRequest req = new DataReadRequest
            {
                CustomKey = ParentId,
                Alias = Id,
                DataType = DataTypes.Output,
                From = from,
                To = to,
                TimeZone = timeZone,
                Granularity = granularity,
                Function = function,
            };
            return ServiceInstance.ReadData(req);
        }

        public LineMetrics.API.DataTypes.Base LoadLastValue()
        {
            LastValueDataReadRequest req = new LastValueDataReadRequest
            {
                CustomKey = ParentId,
                Alias = Id,
                DataType = DataTypes.Output
            };
            return ServiceInstance.DataService.ReadLastValue(ServiceInstance.AuthenticationToken, req);
        }

        public DataWriteResponse SaveData(LineMetrics.API.DataTypes.Base data)
        {
            return SaveData(new List<LineMetrics.API.DataTypes.Base>() { data }).FirstOrDefault();
        }

        public IList<DataWriteResponse> SaveData(IList<LineMetrics.API.DataTypes.Base> data)
        {
            DataWriteRequest writeReq = new DataWriteRequest
            {
                CustomKey = ParentId,
                Alias = Id,
                ObjectId = ObjectId
            };

            foreach (var val in data)
            {
                if (val.GetType() != DataTypes.Input)
                {
                    throw new ServiceException("Input type '{0}' was expected, given '{1}'!", DataTypes.Input.Name, val.GetType().Name);
                }
                writeReq.PayLoad.Add(val);
            }

            return ServiceInstance.WriteData(writeReq);
        }

        public override string ToString()
        {
            return base.ToString() + String.Format(", Alias: {0}, InputId: {1}", Alias, InputId);
        }

        public override string GetId()
        {
            return Id;
        }

        public override string Id
        {
            get
            {
                if (!Alias.IsNullOrWhiteSpace())
                {
                    return Alias;
                }
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
