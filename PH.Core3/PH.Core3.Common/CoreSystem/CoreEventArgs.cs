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
        }

        public IIdentifier Identifier { get; }
    }
}
