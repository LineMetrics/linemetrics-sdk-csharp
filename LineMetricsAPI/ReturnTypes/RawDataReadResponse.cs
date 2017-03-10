using LineMetrics.API.DataTypes;

namespace LineMetrics.API.ReturnTypes
{
    public class RawDataReadResponse : DataReadReponse
    {
        public Base Data { get; set; }

        public RawDataReadResponse(Base data)
        {
            Data = data;
        }

        public override string ToString()
        {
            if (null != Data)
            {
                return Data.ToString();
            }
            else
            {
                return base.ToString();
            }
        }
    }
}
