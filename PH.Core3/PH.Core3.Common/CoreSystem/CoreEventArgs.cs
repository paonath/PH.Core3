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
        protected CoreEventArgs(IIdentifier identifier)
        {
            Identifier = identifier;
            Id = Guid.NewGuid();
            UtcFired = DateTime.UtcNow;
        }

        public IIdentifier Identifier { get; }
        public DateTime UtcFired { get; }
        public Guid Id { get; }
    }
}
