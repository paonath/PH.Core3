using System.Collections.Generic;
using JetBrains.Annotations;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Result with a Not-Found content
    /// Can be on error or a good result.
    /// </summary>
    /// <typeparam name="TContent"></typeparam>
    internal class ResultNotFound<TContent> : Result<TContent> , IResult
    {
        /// <summary>
        /// Init new instance with no error
        /// </summary>
        /// <param name="identifier"></param>
        internal ResultNotFound([NotNull] IIdentifier identifier)
            :base(identifier, null)
        {
            
        }
        
        /// <summary>
        /// Init new instance with errors
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="errors"></param>
        internal ResultNotFound([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
            :base(identifier, errors)
        {
            
        }

    }
}