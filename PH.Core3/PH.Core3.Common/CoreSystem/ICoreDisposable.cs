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
        /// <summary>
        /// Gets a value indicating whether this <see cref="IInitializable{TSelf}"/> is initialized.
        /// </summary>
        /// <value><c>true</c> if initialized; otherwise, <c>false</c>.</value>
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

        /// <summary>Occurs when [disposed evt].</summary>
        event EventHandler<CoreDisposableEventArgs> DisposedEvt;
    }


    
}