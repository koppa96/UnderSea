using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.Exceptions
{

    [Serializable]
    public class LimitReachedException : Exception
    {
        public LimitReachedException() { }
        public LimitReachedException(string message) : base(message) { }
        public LimitReachedException(string message, Exception inner) : base(message, inner) { }
        protected LimitReachedException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
