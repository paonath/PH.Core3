using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using JetBrains.Annotations;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.Common.Services.Components.EF.Crud.Entities
{
    public class EntityValidationContext : CoreDisposable
    {
        private List<ValidationFailure> _failures;

        public EntityValidationContext(IIdentifier identifier)
            :base(identifier)
        {
            _failures = new List<ValidationFailure>();
        }


        public void AddFailure(ValidationFailure failure)
        {
            _failures.Add(failure);
        }



        public async Task AddFailureAsync([NotNull] string propertyName, [NotNull] string errorMessage,
                                          [CanBeNull] string outputMessage = null)
        {
            await Task.Run(() =>
            {
                AddFailure(propertyName, errorMessage, outputMessage);
                return Task.CompletedTask;
            });

        }

        public async Task AddFailureWithValueAsync([NotNull] string propertyName, [NotNull] string errorMessage,
                                                   [NotNull] object attemptedValue,
                                                   [CanBeNull] string outputMessage = null)
        {
            await Task.Run(() =>
            {
                AddFailureWithValue(propertyName, errorMessage, attemptedValue, outputMessage);
                return Task.CompletedTask;
            });
        }

        public void AddFailure([NotNull] string propertyName, [NotNull] string errorMessage,[CanBeNull] string outputMessage = null)
        {
            if (propertyName is null) throw new ArgumentNullException(nameof(propertyName));
            if (errorMessage is null) throw new ArgumentNullException(nameof(errorMessage));
            

            AddFailure(new ValidationFailure(propertyName,errorMessage));
        }

        public void AddFailureWithValue([NotNull] string propertyName, [NotNull] string errorMessage, [NotNull] object attemptedValue,[CanBeNull] string outputMessage = null)
        {
            if (propertyName is null) 
                throw new ArgumentNullException(nameof(propertyName));
            if (errorMessage is null) 
                throw new ArgumentNullException(nameof(errorMessage));
            

            AddFailure(new ValidationFailure(propertyName,errorMessage, attemptedValue));
        }


        internal List<ValidationFailure> GetFailures()
        {
            return _failures;
        }

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