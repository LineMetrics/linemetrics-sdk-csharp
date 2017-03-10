using System.Runtime.Serialization;

namespace LineMetrics.API.DataTypes
{
    [DataContract]
    public class Bool : Base
    {
        [DataMember(Name = "val")]
        private byte _value;

        [IgnoreDataMember]
        public bool Value
        {
            get { return _value != 0; }
            set
            {
                if (value)
                {
                    _value = 1;
                }
                else
                {
                    _value = 0;
                }
            }
        }

        public override string ToString()
        {
            return string.Format("Value: {0}, Timestamp: {1:dd.MM.yyy HH:mm:ss}", Value, Timestamp);
        }
    }
}
