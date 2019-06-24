using System;
using System.Collections.Generic;
using System.Text;

namespace StrategyGame.Bll.Exceptions
{

    [Serializable]
    public class InProgressException : Exception
    {
        public InProgressException() { }
        public InProgressException(string message) : base(message) { }
        public InProgressException(string message, Exception inner) : base(message, inner) { }
        protected InProgressException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
