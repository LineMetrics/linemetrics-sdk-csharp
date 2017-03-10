using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LineMetrics.API.RequestTypes
{
    public class LastValueDataReadRequest : BaseDataReadRequest
    {
        public LastValueDataReadRequest()
        {
            Function = RequestTypes.Function.LAST_VALUE;
        }

        internal override string AppendRequestString()
        {
            return CreateRequestString("function", Function.ToString().ToLowerInvariant());
        }
    }
}
