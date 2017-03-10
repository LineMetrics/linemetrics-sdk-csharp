using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class TemplateRequiredFields
    {
        [DataMember(Name = "data_type")]
        private string dataType;

        [DataMember(Name = "uid")]
        public string Id { get; set; }

        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [IgnoreDataMember]
        public Type DataType
        {
            get
            {
                return Type.GetType("LineMetrics.API.DataTypes." + dataType + ", LineMetrics.API");
            }
            set { dataType = value.Name.ToString(); }
        }

        public override string ToString()
        {
            return String.Format("Id: {0}, Alias: {1}, DataType: {2}", Id, Alias, DataType.Name);
        }
    }
}
