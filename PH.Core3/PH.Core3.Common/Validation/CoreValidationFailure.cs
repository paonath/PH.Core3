using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Validation
{
    /// <summary>
    /// Validation Failure Core
    /// </summary>
    /// <seealso cref="FluentValidation.Results.ValidationFailure" />
    public class CoreValidationFailure : ValidationFailure
    {
        /// <summary>Gets the event identifier.</summary>
        /// <value>The event identifier.</value>
        public EventId? EventId { get; }

        /// <summary>Creates a new validation failure.</summary>
        public CoreValidationFailure(EventId eventId,string propertyName, string errorMessage) : base(propertyName, errorMessage)
        {
            EventId = eventId;
        }
        /// <summary>Creates a new validation failure.</summary>
        public CoreValidationFailure(string propertyName, string errorMessage, EventId? eventId = null) : base(propertyName, errorMessage)
        {
            EventId = eventId;
        }

        /// <summary>Creates a new ValidationFailure.</summary>
        public CoreValidationFailure(EventId eventId,string propertyName, string errorMessage, object attemptedValue) : base(propertyName, errorMessage, attemptedValue)
        {
            EventId = eventId;
        }

        /// <summary>Creates a new ValidationFailure.</summary>
        public CoreValidationFailure(string propertyName, string errorMessage, object attemptedValue, EventId? eventId = null) : base(propertyName, errorMessage, attemptedValue)
        {
            EventId = eventId;
        }

        /// <summary>Parses the data annotation failure.</summary>
        /// <param name="failure">The failure.</param>
        /// <returns>CoreValidationFailure</returns>
        public static CoreValidationFailure ParseDataAnnotationFailure(System.ComponentModel.DataAnnotations.ValidationResult failure)
        {
            return new CoreValidationFailure(failure.MemberNames.FirstOrDefault(),
                                             failure.ErrorMessage);
        }

    }
}