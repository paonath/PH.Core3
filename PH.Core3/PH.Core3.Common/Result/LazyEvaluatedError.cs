using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Result
{

    /// <summary>
    /// Error instance related to a lazy-evaluated function
    ///
    /// <see cref="ResultFactory.FailLazyEvaluatedFunction{TContent}"/>
    /// </summary>
    internal class LazyEvaluatedError : Error
    {
        /// <summary>
        /// Int identifier of function in error
        /// </summary>
        public int ProgrId { get; }

        /// <summary>
        /// Init new instance of LazyEvaluatedError
        /// </summary>
        /// <param name="progrId">Int identifier of function in error</param>
        /// <param name="errorMessage">Error message (useful for logging)</param>
        /// <param name="outputMessage">Optional Message to Service that received the error</param>
        /// <param name="errorEventId">Event Id for this error</param>
        /// <param name="innerError">Inner Error</param>
        public LazyEvaluatedError(int progrId,[NotNull] string errorMessage, [CanBeNull] string outputMessage, EventId? errorEventId,  [CanBeNull] IError innerError = null) 
            : base(errorMessage, outputMessage, errorEventId, innerError)
        {
            ProgrId = progrId;
        }
    }
}