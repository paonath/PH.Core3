using System;
using JetBrains.Annotations;

namespace PH.Core3.Common.CoreSystem
{
    /// <summary>
    /// A service that require initialize
    /// </summary>
    /// <typeparam name="TSelf">Self Concrete-Type</typeparam>
    public interface IInitializable<out TSelf>
    {
        bool Initialized { get; }

        /// <summary>
        /// Init Method
        /// </summary>
        /// <returns>Instance of initialized Service</returns>
        [NotNull]
        TSelf Initialize();
    }


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