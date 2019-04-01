using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Settings;

namespace PH.Core3.Common.Services.Components.EF
{
    /// <summary>
    /// Setting/Configuration Transient service for CRUD Entity/Dto
    /// </summary>
    public class TransientCrudSettings : ServiceBase 
    {
       
        /// <summary>
        /// Initialize a new instance of <see cref="TransientCrudSettings"/> with all settings to True
        /// </summary>
        /// <param name="coreIdentifier">Service Identifier</param>
        /// <param name="logger">Service Logger</param>
        public TransientCrudSettings([NotNull] IIdentifier coreIdentifier, [NotNull] ILogger<TransientCrudSettings> logger) 
            : this(coreIdentifier, logger,true,true,true,true)
        {
        }


        /// <summary>
        /// Initialize a new instance of <see cref="TransientCrudSettings"/>
        /// </summary>
        /// <param name="coreIdentifier">Service Identifier</param>
        /// <param name="logger">Service Logger</param>
        /// <param name="autoSaveChanges">value fot auto-save changes</param>
        /// <param name="insertValidationEnabled">value for validation on insert</param>
        /// <param name="updateValidationEnabled">value for validation on update</param>
        /// <param name="deleteValidationEnabled">value for validation on delete</param>
        public TransientCrudSettings([NotNull] IIdentifier coreIdentifier, [NotNull] ILogger<TransientCrudSettings> logger
                                     , bool autoSaveChanges = true, bool insertValidationEnabled = true, bool updateValidationEnabled = true, bool deleteValidationEnabled = true)
            : base(coreIdentifier)
        {
            AutoSaveChanges         = new SettingVariable<bool>(autoSaveChanges);
            InsertValidationEnabled = new SettingVariable<bool>(insertValidationEnabled);
            UpdateValidationEnabled = new SettingVariable<bool>(updateValidationEnabled);
            DeleteValidationEnabled = new SettingVariable<bool>(deleteValidationEnabled);
        }


         

        /// <summary>
        /// If True the Service perform SaveChanges after every Add/Update/Delete.
        ///
        /// If False the programmer have to explicit call <see cref="ContextServiceBase{TContext}.FlushChangesAsync"/>
        /// </summary>
        public SettingVariable<bool> AutoSaveChanges { get; set; }

        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> InsertValidationEnabled { get; set; }
        
        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> UpdateValidationEnabled { get; set; }
        
        /// <summary>
        /// If False do not perform a validation and return a valid result
        /// </summary>
        public SettingVariable<bool> DeleteValidationEnabled { get; set; }


    }
}