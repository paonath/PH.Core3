using System;
using System.Collections.Generic;
using System.Text;

namespace PH.Core3.Common.CoreSystem
{

    /// <summary>
    /// Base Event Argument class
    /// </summary>
    public abstract class CoreEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreEventArgs"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        protected CoreEventArgs(IIdentifier identifier)
        {
            Identifier = identifier;
            Id = Guid.NewGuid();
            UtcFired = DateTime.UtcNow;
        }
        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        public IIdentifier Identifier { get; }

        /// <summary>Gets the UTC fired.</summary>
        /// <value>The UTC fired.</value>
        public DateTime UtcFired { get; }

        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        public Guid Id { get; }
    }
}
