using System.Collections.Generic;

namespace PH.Core3.Common.Result
{
    ///// <summary>
    ///// Result
    ///// </summary>
    //public interface IResult
    //{
    //    /// <summary>
    //    /// Gets a value indicating whether on error.
    //    /// </summary>
    //    /// <value><c>true</c> if on error; otherwise, <c>false</c>.</value>
    //    bool OnError { get; }

    //    /// <summary>
    //    /// List of Errors, empty id result is Ok
    //    /// </summary>
    //    List<IError> Errors { get; }

    //    /// <summary>Gets the identifier.</summary>
    //    /// <value>The identifier.</value>
    //    IIdentifier Identifier { get; }


    //}




    ///// <summary>
    ///// Transport object wrapping a Result
    ///// </summary>
    ///// <typeparam name="TContent">Type of the Result content</typeparam>
    //public interface IResult<out TContent> : IResult
    //{
    //    /// <summary>
    //    /// Result Content
    //    /// </summary>
    //    TContent Content { get; }
    //}

    /// <summary>
    /// Transport object result wrapping a contents
    /// </summary>
    /// <typeparam name="T">Type of the Result content</typeparam>
    public interface IResult<out T>
    {
        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        IIdentifier Identifier { get; }

        
        /// <summary>
        /// Gets a value indicating whether on error.
        /// </summary>
        /// <value><c>true</c> if on error; otherwise, <c>false</c>.</value>
        bool OnError { get; }

        /// <summary>Gets the error, if any.</summary>
        /// <value>The error.</value>
        IError Error { get; }

        /// <summary>
        /// Result Content
        /// </summary>
        T Content { get; }
    }
}