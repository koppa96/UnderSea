using System;
using System.Runtime.Serialization;

namespace StrategyGame.Bll.Exceptions
{

    /// <summary>
    /// The exception that is thrown when there is already a building or research in progress, preventing the start of another.
    /// </summary>
    [Serializable]
    public class InProgressException : Exception
    {
        public InProgressException() { }
        public InProgressException(string message)
            : base(message) { }
        public InProgressException(string message, Exception inner)
            : base(message, inner) { }
        protected InProgressException(
          SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}