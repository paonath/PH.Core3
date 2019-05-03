using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Identifiers.Services;
using PH.Core3.Common.Settings;

namespace PH.Core3.Common.Services.Components.Crud
{
    /// <summary>
    /// Setting/Configuration Transient service for CRUD Entity/Dto
    /// </summary>
    public class TransientCrudSettings : TransientReadSettings
    {
        /// <summary>
        /// Initialize a new instance of <see cref="TransientCrudSettings"/> with all settings to True
        /// </summary>
        /// <param name="coreIdentifier">Service Identifier</param>
        /// <param name="logger">Service Logger</param>
        public TransientCrudSettings([NotNull] IIdentifier coreIdentifier,
                                     [NotNull] ILogger<TransientCrudSettings> logger)
            : this(coreIdentifier, logger, true, true, true, true, 128)
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
        /// <param name="itemsPaginationSize">value for pagination, -1 to disable</param>
        public TransientCrudSettings([NotNull] IIdentifier coreIdentifier,
                                     [NotNull] ILogger<TransientCrudSettings> logger
                                     , bool autoSaveChanges = true, bool insertValidationEnabled = true,
                                     bool updateValidationEnabled = true, bool deleteValidationEnabled = true,
                                     int itemsPaginationSize = 128)
            : base(coreIdentifier, itemsPaginationSize)
        {
            AutoSaveChanges         = new SettingVariable<bool>(autoSaveChanges);
            InsertValidationEnabled = new SettingVariable<bool>(insertValidationEnabled);
            UpdateValidationEnabled = new SettingVariable<bool>(updateValidationEnabled);
            DeleteValidationEnabled = new SettingVariable<bool>(deleteValidationEnabled);
            
        }


        /// <summary>
        /// If True the Service perform SaveChanges after every Add/Update/Delete.
        ///
        /// If False the programmer have to explicit call ContextServiceBase{TContext}.FlushChangesAsync
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

        

        /// <summary>
        /// Service Identifier (a int value representing the service and the service name)
        /// </summary>
        public override ServiceIdentifier ServiceIdentifier => ConstSettings.TransientCrudConsts.ServiceIdentifier;
    }
}