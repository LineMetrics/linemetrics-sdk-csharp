using System;
using System.Runtime.Serialization;

namespace LineMetrics.API.ReturnTypes
{
    [DataContract]
    public class DataStreamType
    {
        [DataMember(Name = "input")]
        private string input;

        [DataMember(Name = "output")]
        private string output;

        [IgnoreDataMember]
        public Type Input
        {
            get
            {
                return Type.GetType("LineMetrics.API.DataTypes." + input + ", LineMetrics.API");
            }
            set { input = value.Name.ToString(); }
        }

        [IgnoreDataMember]
        public Type Output
        {
            get
            {
                return Type.GetType("LineMetrics.API.DataTypes." + output + ", LineMetrics.API");
            }
            set { output = value.Name.ToString(); }
        }

        public override string ToString()
        {
            return String.Format("Input: {0}, Output: {1}", input, output);
        }
    }
}
