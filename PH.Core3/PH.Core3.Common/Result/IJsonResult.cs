//namespace PH.Core3.Common.Result
//{
//    /// <summary>
//    /// Result from a Json result
//    /// </summary>
//    /// <seealso cref="PH.Core3.Common.Result.IResult" />
//    public interface IJsonResult : IResult
//    {
//        /// <summary>Gets the content of the json.</summary>
//        /// <value>The content of the json.</value>
//        string JsonContent { get;  }
//    }

//    /// <summary>
//    /// Transport object wrapping a Result and a json content
//    /// </summary>
//    /// <typeparam name="TContent">The type of the content.</typeparam>
//    /// <seealso cref="PH.Core3.Common.Result.IResult" />
//    public interface IJsonResult<out TContent> : IJsonResult, IResult<TContent>
//    {

//    }
//}