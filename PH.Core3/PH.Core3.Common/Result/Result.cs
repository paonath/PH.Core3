using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using JetBrains.Annotations;

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

    internal class ResultEmpty : Result<object> , IResult
    {
        internal ResultEmpty([NotNull] IIdentifier identifier) : base(identifier, new object())
        {
        }

        internal ResultEmpty([NotNull] IIdentifier identifier, [NotNull] IEnumerable<IError> errors) : base(identifier, errors)
        {
        }
    }

    public static class Result
    {
        public static IResult Ok([NotNull] IIdentifier identifier)
        {
            return new ResultEmpty(identifier);
        }

        public static IResult<TContent> Ok<TContent>([NotNull] IIdentifier identifier, TContent content)
        {
            return new Result<TContent>(identifier, content);
        }

        public static IResult Fail([NotNull] IIdentifier identifier, IEnumerable<IError> errors)
        {
            return new ResultEmpty(identifier, errors);
        }

        public static IResult<TContent> Fail<TContent>([NotNull] IIdentifier identifier, IEnumerable<IError> errors)
        {
            return new Result<TContent>(identifier, errors);
        }

    }
}
