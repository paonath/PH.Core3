using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Result
{
    internal class Result<TContent> : IResult<TContent>
    {
        internal Result()
        {
            Errors = new List<IError>();
        }

        internal Result([NotNull] IIdentifier identifier,[NotNull] TContent content)
        {
            Content = content;
            Identifier = identifier;
        }

        internal Result([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            Identifier = identifier;
            Errors = errors.ToList();
        }


        /// <summary>
        /// True if On Error
        /// </summary>
        public bool OnError => Errors != null && Errors.Count > 0;

        /// <summary>
        /// List of Errors, empty id result is Ok
        /// </summary>
        public List<IError> Errors { get; }
        public IIdentifier Identifier { get; }

        /// <summary>
        /// Result Content
        /// </summary>
        public TContent Content { get; }
    }

    internal class ResultEmpty : Result<bool> , IResult
    {
        internal ResultEmpty([NotNull] IIdentifier identifier) : base(identifier, true)
        {
        }

        internal ResultEmpty([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
        {
        }
    }

    public static class ResultFactory
    {

        [NotNull]
        public static IResult True([NotNull] IIdentifier identifier)
        {
            return new ResultEmpty(identifier);
        }

        [NotNull]
        public static IResult Ok([NotNull] IIdentifier identifier)
        {
            return new ResultEmpty(identifier);
        }

        [NotNull]
        public static IResult<TContent> Ok<TContent>([NotNull] IIdentifier identifier, [NotNull] TContent content)
        {
            return new Result<TContent>(identifier, content);
        }

        [NotNull]
        public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            return new ResultEmpty(identifier, errors);
        }
        [NotNull]
        public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            return new ResultEmpty(identifier, new []{error});
        }

        [NotNull]
        public static IResult Fail([NotNull] IIdentifier identifier, [NotNull] string errorMessage,EventId? eventId = null,
                                   string outputMessage = "")
        {
            return new ResultEmpty(identifier, new[] {new Error(errorMessage,outputMessage,eventId)});
        }

        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors)
        {
            return new Result<TContent>(identifier, errors);
        }
        
        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] IError error)
        {
            return new Result<TContent>(identifier, new []{error});
        }

        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, [NotNull] string errorMessage,
                                                       string outputMessage = "",EventId? eventId = null)
        {
            return new Result<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) }); 
        }

        [NotNull]
        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, EventId eventId, [NotNull] string errorMessage,
                                                       string outputMessage = "")
        {
            return new Result<TContent>(identifier, new []{new Error(errorMessage,outputMessage, eventId) }); 
        }


    }
}
