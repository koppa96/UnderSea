using System;

namespace StrategyGame.Model.Entities.Logging
{

    /// <summary>
    /// Represents a request log in the UnderSea database.
    /// </summary>
    public class RequestLog : AbstractEntity<RequestLog>
    {
        /// <summary>
        /// Gets or sets the HTTP method of the request.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Gets or sets the target URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the time when the request occured.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the response status code.
        /// </summary>
        public int ResponseStatus { get; set; }
    }
}