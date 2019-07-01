using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Model.Entities.Logging
{
    public class RequestLog : AbstractEntity<RequestLog>
    {
        public string Method { get; set; }
        public string Url { get; set; }
        public DateTime Timestamp { get; set; }
        public int ResponseStatus { get; set; }
    }
}
