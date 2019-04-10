using System.Collections.Generic;

namespace PH.Core3.Common.Result
{
    /// <summary>
    /// Result
    /// </summary>
    public interface IResult
    {
        /// <summary>
        /// True if On Error
        /// </summary>
        bool OnError { get; }

        /// <summary>
        /// List of Errors, empty id result is Ok
        /// </summary>
        List<IError> Errors { get; }

        IIdentifier Identifier { get; }


    }


    /// <summary>
    /// Transport object wrapping a Result
    /// </summary>
    /// <typeparam name="TContent">Type of the Result content</typeparam>
    public interface IResult<out TContent> : IResult
    {
        /// <summary>
        /// Result Content
        /// </summary>
        TContent Content { get; }
    }
}