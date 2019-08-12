using Microsoft.Extensions.Logging;
using PH.Core3.Common.Bus;
using PH.Core3.Common.UnitOfWorkInfrastructure;
using PH.Core3.UnitOfWork;

namespace PH.Core3.EntityFramework
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

    /// <summary>
    /// Automatic Perform Flush on Commit
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Bus.TinyActionBus" />
    /// <seealso cref="PH.Core3.EntityFramework.ICommittableActionBus" />
    public class CommittableActionBus : TinyActionBus , ICommittableActionBus
    {
        private readonly IUnitOfWork _uow;
        /// <summary>
        /// Initializes a new instance of the <see cref="TinyActionBus"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public CommittableActionBus(ILogger<CommittableActionBus> logger, IUnitOfWork uow) : base(logger)
        {
            _uow = uow;
            _uow.Committed += UowOnCommitted;
            ThrowOnException = false;
        }

        private void UowOnCommitted(object sender, UnitOfWorkEventArg e)
        {
            Flush(ThrowOnException);
        }

        /// <summary>
        /// Gets or sets a value indicating whether <c>true</c>throw exception on error.
        /// </summary>
        /// <value><c>true</c> if throw on exception; otherwise, <c>false</c>.</value>
        public bool ThrowOnException { get; set; }
    }
}