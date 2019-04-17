using System.Collections.Generic;
using JetBrains.Annotations;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Result Boolean
    /// </summary>
    internal class ResultEmpty : Result<bool> , IResult
    {
        /// <summary>
        /// Ok Constructor
        /// </summary>
        /// <param name="identifier">Identifier</param>
        internal ResultEmpty([NotNull] IIdentifier identifier) : base(identifier, true)
        {
        }

        /// <summary>
        /// Bad Constructor
        /// </summary>
        /// <param name="identifier">Identifier</param>
        /// <param name="errors">errors</param>
        internal ResultEmpty([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
        {
            Content = false;
        }
    }
}