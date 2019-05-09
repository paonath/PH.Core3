using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using JetBrains.Annotations;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.Common.Validation
{
    /// <summary>
    /// Core Exception caused by Invalid Data
    /// </summary>
    /// <seealso cref="PH.Core3.Common.CoreSystem.CoreException" />
    public class CoreValidationException : CoreException
    {
        /// <summary>
        /// Gets the validation failures.
        /// </summary>
        /// <value>
        /// The validation failures.
        /// </value>
        public List<CoreValidationFailure> ValidationFailures { get; private set; }

        private void InitValidationFailures([NotNull] IEnumerable<CoreValidationFailure> validationFailures)
        {
            ValidationFailures = validationFailures.ToList();
            foreach (var failure in ValidationFailures)
            {
                Data.Add(failure.PropertyName, failure.ErrorMessage);
            }
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreValidationException"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="validationFailures">The validation failures.</param>
        public CoreValidationException(IIdentifier identifier, IEnumerable<CoreValidationFailure> validationFailures) : base(identifier)
        {
            InitValidationFailures(validationFailures);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreValidationException"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="validationFailures">The validation failures.</param>
        public CoreValidationException(IIdentifier identifier, string message, IEnumerable<CoreValidationFailure> validationFailures) : base(identifier, message)
        {
            InitValidationFailures(validationFailures);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CoreValidationException"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="message">The message.</param>
        /// <param name="validationFailures">The validation failures.</param>
        /// <param name="innerException">The inner exception.</param>
        public CoreValidationException(IIdentifier identifier, string message, IEnumerable<CoreValidationFailure> validationFailures, Exception innerException) : base(identifier, message, innerException)
        {
            InitValidationFailures(validationFailures);
        }

        /// <summary>
        /// Parses the data annotation errors and return instance of CoreValidationException
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="errors">The errors.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>CoreValidationException</returns>
        public static CoreValidationException ParseDataAnnotationErrors([NotNull] IIdentifier identifier
                                                                        ,[NotNull] IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> errors
                                                                        , [CanBeNull] string message = null
                                                                        , [CanBeNull] Exception innerException = null)
        {
            

            var l = errors.Select(x => CoreValidationFailure.ParseDataAnnotationFailure(x)).ToList();
            return new CoreValidationException(identifier, message, l, innerException);

            

            
        }
    }
}