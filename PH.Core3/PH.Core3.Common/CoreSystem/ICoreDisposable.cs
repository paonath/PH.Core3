using System;

namespace PH.Core3.Common.CoreSystem
{
    /// <summary>
    /// Provides a mechanism for releasing unmanaged resources.
    ///
    /// <see cref="IDisposable"/>
    /// </summary>
    public interface ICoreDisposable : IDisposable
    {
        /// <summary>
        /// If Dispose already performed
        /// </summary>
        bool Disposed { get; }

        event EventHandler<CoreDisposableEventArgs> DisposedEvt;
    }
}