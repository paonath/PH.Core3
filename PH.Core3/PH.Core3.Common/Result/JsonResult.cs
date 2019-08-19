//using System.Collections.Generic;
//using JetBrains.Annotations;

//namespace PH.Core3.Common.Result
//{
//    internal class JsonResult<TContent> : Result<TContent>, IJsonResult<TContent>
//    {
//        /// <summary>
//        /// Init new instance of result with no error
//        /// </summary>
//        /// <param name="identifier">Identifier</param>
//        /// <param name="content">Content</param>
//        /// <param name="jsonContent">Json Content</param>
//        internal JsonResult([NotNull] IIdentifier identifier, [NotNull] TContent content, string jsonContent) : base(identifier, content)
//        {
//            JsonContent = jsonContent;
//        }

//        /// <summary>
//        /// Init new instance of result with errors
//        /// </summary>
//        /// <param name="identifier">Identifier</param>
//        /// <param name="errors">errors </param>
//        internal JsonResult([NotNull] IIdentifier identifier, [CanBeNull] IEnumerable<IError> errors) : base(identifier, errors)
//        {
//            JsonContent = null;
//        }

//        /// <summary>
//        /// Init new instance of result with errors
//        /// </summary>
//        /// <param name="identifier">Identifier</param>
//        /// <param name="errors">errors </param>
//        /// <param name="jsonContent">Json Content</param>
//        internal JsonResult([NotNull] IIdentifier identifier, [CanBeNull] IEnumerable<IError> errors, string jsonContent) : base(identifier, errors)
//        {
//            JsonContent = jsonContent;
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="JsonResult{TContent}"/> class.
//        /// </summary>
//        /// <param name="content">The content.</param>
//        /// <param name="errors">The errors.</param>
//        /// <param name="identifier">The identifier.</param>
//        public JsonResult([NotNull] TContent content, [NotNull] List<IError> errors, [NotNull] IIdentifier identifier) : base(content, errors, identifier)
//        {
//            JsonContent = null;
//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="JsonResult{TContent}"/> class.
//        /// </summary>
//        /// <param name="content">The content.</param>
//        /// <param name="errors">The errors.</param>
//        /// <param name="identifier">The identifier.</param>
//        /// <param name="jsonContent">Content of the json.</param>
//        public JsonResult([NotNull] TContent content, [NotNull] List<IError> errors, [NotNull] IIdentifier identifier, string jsonContent) : base(content, errors, identifier)
//        {
//            JsonContent = jsonContent;
//        }

//        /// <summary>Gets the content of the json.</summary>
//        /// <value>The content of the json.</value>
//        public string JsonContent { get; }
//    }
//}