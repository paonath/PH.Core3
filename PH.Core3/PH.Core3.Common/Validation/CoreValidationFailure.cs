using FluentValidation.Results;
using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Validation
{
    public class CoreValidationFailure : ValidationFailure
    {
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
    }
}