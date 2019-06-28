using System;
using System.Runtime.Serialization;

namespace StrategyGame.Bll.Exceptions
{
    /// <summary>
    /// The exception that is thrown when the unit limit is reached.
    /// </summary>
    [Serializable]
    public class LimitReachedException : Exception
    {
        public LimitReachedException() { }
        public LimitReachedException(string message)
            : base(message) { }
        public LimitReachedException(string message, Exception inner)
            : base(message, inner) { }
        protected LimitReachedException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}