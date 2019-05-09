using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Validation;

namespace PH.Core3.Common.Services.Components.Crud.Entities
{

    

    /// <summary>
    /// Validation context
    /// </summary>
    public class EntityValidationContext : CoreDisposable
    {
        private List<CoreValidationFailure> _failures;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifier"></param>
        public EntityValidationContext(IIdentifier identifier)
            :base(identifier)
        {
            _failures = new List<CoreValidationFailure>();
        }


        /// <summary>Performs the data annotation validation.</summary>
        /// <param name="objectToValidate">The object to validate.</param>
        public void PerformDataAnnotationValidation([NotNull] object objectToValidate)
        {
            if (!DataAnnotationsValidator.TryValidate(objectToValidate, out var failures))
            {
                _failures.AddRange(failures.Select(x => CoreValidationFailure.ParseDataAnnotationFailure(x)));
            }
        }


        /// <summary>Performs the data annotation validation.</summary>
        /// <param name="objectToValidate">The object to validate.</param>
        [NotNull]
        public Task PerformDataAnnotationValidationAsync([NotNull] object objectToValidate)
        {
            PerformDataAnnotationValidation(objectToValidate);
            return Task.CompletedTask;
        }




        /// <summary>
        /// Add a failure
        /// </summary>
        /// <param name="failure">failure</param>
        public void AddFailure(CoreValidationFailure failure)
        {
            _failures.Add(failure);
        }



        /// <summary>
        /// add async failure
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>awaitable task</returns>
        public Task AddFailureAsync([NotNull] string propertyName, [NotNull] string errorMessage,
                                          [CanBeNull] string outputMessage = null)
        {
        
            AddFailure(propertyName, errorMessage, outputMessage);
            return Task.CompletedTask;
      

        }

        /// <summary>
        /// add async failure
        /// </summary>
        /// <param name="eventId">event id</param>
        /// <param name="propertyName">property name</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>awaitable task</returns>
        public Task AddFailureAsync(EventId eventId,[NotNull] string propertyName, [NotNull] string errorMessage,
                                          [CanBeNull] string outputMessage = null)
        {
           
                AddFailure(eventId,propertyName, errorMessage, outputMessage);
                return Task.CompletedTask;
            

        }


        /// <summary>
        /// add async failure with current value
        /// </summary>
        /// <param name="propertyName">property name</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="attemptedValue">attempted value on error</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>awaitable task</returns>
        public Task AddFailureWithValueAsync([NotNull] string propertyName, [NotNull] string errorMessage,
                                                   [NotNull] object attemptedValue,
                                                   [CanBeNull] string outputMessage = null)
        {
            
                AddFailureWithValue(propertyName, errorMessage, attemptedValue, outputMessage);
                return Task.CompletedTask;
            
        }

        /// <summary>
        /// add async failure with current value
        /// </summary>
        /// <param name="eventId">event id</param>
        /// <param name="propertyName">property name</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="attemptedValue">attempted value on error</param>
        /// <param name="outputMessage">output message</param>
        /// <returns>awaitable task</returns>
        public Task AddFailureWithValueAsync(EventId eventId,[NotNull] string propertyName, [NotNull] string errorMessage,
                                                   [NotNull] object attemptedValue,
                                                   [CanBeNull] string outputMessage = null)
        {
            
                AddFailureWithValue(eventId,propertyName, errorMessage, attemptedValue, outputMessage);
                return Task.CompletedTask;
           
        }

        
        /// <summary>
        /// add failure
        /// </summary>
        /// <param name="eventId">event id</param>
        /// <param name="propertyName">property name</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="outputMessage">output message</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddFailure(EventId eventId,[NotNull] string propertyName, [NotNull] string errorMessage,[CanBeNull] string outputMessage = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (errorMessage is null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }


            AddFailure(new CoreValidationFailure(eventId,propertyName,errorMessage));
        }

        /// <summary>Adds the failure.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <exception cref="ArgumentNullException">
        /// propertyName
        /// or
        /// errorMessage
        /// </exception>
        public void AddFailure([NotNull] string propertyName, [NotNull] string errorMessage,[CanBeNull] string outputMessage = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (errorMessage is null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }


            AddFailure(new CoreValidationFailure(propertyName,errorMessage));
        }

        /// <summary>Adds the failure with value.</summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="errorMessage">The error message.</param>
        /// <param name="attemptedValue">The attempted value.</param>
        /// <param name="outputMessage">The output message.</param>
        /// <exception cref="ArgumentNullException">
        /// propertyName
        /// or
        /// errorMessage
        /// </exception>
        public void AddFailureWithValue([NotNull] string propertyName, [NotNull] string errorMessage, [NotNull] object attemptedValue,[CanBeNull] string outputMessage = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (errorMessage is null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }


            AddFailure(new CoreValidationFailure(propertyName,errorMessage, attemptedValue));
        }

        /// <summary>
        /// add a failure
        /// </summary>
        /// <param name="eventId">event id</param>
        /// <param name="propertyName">property name</param>
        /// <param name="errorMessage">error message</param>
        /// <param name="attemptedValue">attempted value on error</param>
        /// <param name="outputMessage">output message</param>
        /// <exception cref="ArgumentNullException"></exception>
        public void AddFailureWithValue(EventId eventId,[NotNull] string propertyName, [NotNull] string errorMessage, [NotNull] object attemptedValue,[CanBeNull] string outputMessage = null)
        {
            if (propertyName is null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            if (errorMessage is null)
            {
                throw new ArgumentNullException(nameof(errorMessage));
            }


            AddFailure(new CoreValidationFailure(eventId,propertyName,errorMessage, attemptedValue));
        }


        internal List<CoreValidationFailure> GetFailures()
        {
            return _failures;
        }

        /// <summary>
        /// return validation results
        /// </summary>
        /// <returns>Validation result</returns>
        [NotNull]
        public Task<ValidationResult> GetValidationResult()
        {
            return Task.FromResult(new ValidationResult(GetFailures()));
            
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected override void Dispose(bool disposing)
        {
            _failures = null;
        }

       
    }
}