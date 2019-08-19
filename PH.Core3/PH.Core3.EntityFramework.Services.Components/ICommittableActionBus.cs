using PH.Core3.Common.Bus;

namespace PH.Core3.EntityFramework.Services.Components
{
    /// <summary>
    /// Automatic Perform Flush on Commit - Require IUnitOfWork on implementation
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Bus.IActionBus" />
    public interface ICommittableActionBus : IActionBus
    {
        /// <summary>
        /// Gets or sets a value indicating whether <c>true</c>throw exception on error.
        /// </summary>
        /// <value><c>true</c> if throw on exception; otherwise, <c>false</c>.</value>
        bool ThrowOnException { get; set; }
    }
}