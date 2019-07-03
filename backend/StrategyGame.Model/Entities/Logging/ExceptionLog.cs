using System;

namespace StrategyGame.Model.Entities.Logging
{
    /// <summary>
    /// Represents an exception log in the UnderSea database.
    /// </summary>
    public class ExceptionLog : AbstractEntity<ExceptionLog>
    {
        /// <summary>
        /// Gets or sets the time when the exception was thrown.
        /// </summary>
        public DateTime ThrownAt { get; set; }

        /// <summary>
        /// Gets or sets the type of the exception.
        /// </summary>
        public string ExceptionType { get; set; }

        /// <summary>
        /// Gets or sets the message of th exception.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the stack trace of the exception.
        /// </summary>
        public string StackTrace { get; set; }
    }
}