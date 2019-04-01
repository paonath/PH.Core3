using System;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Models.Entities;
using PH.Core3.Common.Settings;

namespace PH.Core3.Common.Services.Components.EF.Crud.Entities
{
    /// <summary>
    /// </summary>
    /// <typeparam name="TEntity">Type Of Entity</typeparam>
    /// <typeparam name="TKey">Type of Entity Id Property</typeparam>
    public class EntityValidator<TEntity, TKey> : AbstractValidator<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        //private readonly CancellationToken _cancellationToken;
        internal EntityValidationContext ValidationContext { get; private set; }


        private readonly
            Func<TEntity, EntityValidationContext, Task<ValidationResult>> _funcOnCreateValidation;

        private readonly
            Func<TEntity, EntityValidationContext, Task<ValidationResult>> _funcOnUpdateValidation;

        private readonly
            Func<TEntity, EntityValidationContext, Task<ValidationResult>> _funcOnDeleteValidation;

        private readonly ILogger _logger;

        private readonly TransientCrudSettings _crudSettings;

        
        internal EntityValidator([NotNull] IIdentifier identifier
                                 , [NotNull] Func<TEntity, EntityValidationContext, Task<ValidationResult>> funcOnCreateValidation
                                 , [NotNull] Func<TEntity, EntityValidationContext, Task<ValidationResult>> funcOnUpdateValidation
                                 , [NotNull] Func<TEntity, EntityValidationContext, Task<ValidationResult>> funcOnDeleteValidation
                                 , [NotNull] ILogger logger
                                 , [NotNull] TransientCrudSettings settings

        )
        {
            
            _funcOnCreateValidation =
                funcOnCreateValidation ?? throw new ArgumentNullException(nameof(funcOnCreateValidation));
            _funcOnUpdateValidation =
                funcOnUpdateValidation ?? throw new ArgumentNullException(nameof(funcOnUpdateValidation));
            _funcOnDeleteValidation =
                funcOnDeleteValidation ?? throw new ArgumentNullException(nameof(funcOnDeleteValidation));


            _logger       = logger ?? throw new ArgumentNullException(nameof(logger));
            _crudSettings = settings ?? throw new ArgumentNullException(nameof(settings));



            CoreIdentifier = identifier;

            ValidationContext = new EntityValidationContext(identifier);

        }


        

        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> InsertValidationEnabled => _crudSettings.InsertValidationEnabled;

        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> UpdateValidationEnabled => _crudSettings.UpdateValidationEnabled;


        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> DeleteValidationEnabled => _crudSettings.DeleteValidationEnabled;

        
        /// <summary>
        /// Validate on Insert
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>validation result</returns>
        public async Task<ValidationResult> ValidateInsertAsync(TEntity entity)
        {
            if (InsertValidationEnabled.Value)
            {
                var result = await _funcOnCreateValidation.Invoke(entity, ValidationContext);
                return result;
            }

            return GetNotPerformedValidation(ValidationType.OnCreate);
        }

        [NotNull]
        private ValidationResult GetNotPerformedValidation(string operation)
        {
            _logger.LogWarning($"Validation disabled: return a Valid Result for '{operation}'");
            return new ValidationResult();
        }

        
        /// <summary>
        /// Validate on Update
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>validation result</returns>
        public async Task<ValidationResult> ValidateUpdateAsync(TEntity entity)
        {
            if (UpdateValidationEnabled.Value)
            {
                var result = await _funcOnUpdateValidation.Invoke(entity, ValidationContext);
                return result;
            }

            return GetNotPerformedValidation(ValidationType.OnUpdate);
        }

        
        /// <summary>
        /// Validate on Delete
        /// </summary>
        /// <param name="entity">Entity to validate</param>
        /// <returns>validation result</returns>
        public async Task<ValidationResult> ValidateDeleteAsync(TEntity entity)
        {
            if (DeleteValidationEnabled.Value)
            {
                var result = await _funcOnDeleteValidation.Invoke(entity, ValidationContext);
                return result;
            }

            return GetNotPerformedValidation(ValidationType.OnDelete);
        }

       
        public IIdentifier CoreIdentifier { get; }
    }
}