using PH.Core3.Common.Identifiers.Services;
using PH.Core3.Common.Settings;

namespace PH.Core3.Common.Services.Components.Crud
{
    /// <summary>
    /// Read Settings
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Services.Components.ServiceBase" />
    public class TransientReadSettings : ServiceBase
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientReadSettings"/> class.
        /// </summary>
        /// <param name="identifier">Per Scope Transient Identifier</param>
        public TransientReadSettings(IIdentifier identifier)
            :this(identifier, 128)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransientReadSettings"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <param name="itemsPaginationSize">Size of the items pagination.</param>
        public TransientReadSettings(IIdentifier identifier,int itemsPaginationSize = 128) : base(identifier)
        {
            ItemsPaginationSize = new SettingVariable<int>(itemsPaginationSize);
        }

        /// <summary>
        /// Service Identifier (a int value representing the service and the service name)
        /// </summary>
        public override ServiceIdentifier ServiceIdentifier => ConstSettings.TransientReadConsts.ServiceIdentifier;

        /// <summary>
        /// Get or Set se Number of items retrieved by a LoadAll, or other paginated method.
        /// -1 For disabling pagination
        /// </summary>
        public SettingVariable<int> ItemsPaginationSize { get; set; }
    }
}