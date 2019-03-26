using System;
using JetBrains.Annotations;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.Common.Scope
{
    /// <summary>
    /// A Null Scope
    /// </summary>
    public sealed class NullScope : IDisposable
    {
        private NullScope()
        {
            Disposed = false;
        }
        
        /// <summary>
        /// new Instance of <see cref="NullScope"/>
        /// </summary>
        [NotNull]
        public static NullScope Instance => new NullScope();


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// If Dispose already performed
        /// </summary>
        public bool Disposed { get; private set; }
    }
}