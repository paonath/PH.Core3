using System;
using System.Collections.Generic;
using System.Text;

namespace PH.Core3.Common.CoreSystem
{
    /// <summary>
    /// Base abstract class implementing <see cref="ICoreDisposable"/>
    /// 
    /// </summary>
    public abstract class CoreDisposable : ICoreDisposable
    {
        public static ICoreDisposable Instance(IIdentifier identifier)
        {
            return new CoreDisposableInstance(identifier);
        }

        private IIdentifier _identifier;

        /// <summary>
        /// Initialize a new instance of <see cref="CoreDisposable"/>
        /// </summary>
        protected CoreDisposable(IIdentifier identifier)
        {
            _identifier = identifier;
            Disposed = false;
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected abstract void Dispose(bool disposing);


        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public virtual void Dispose()
        {
            Dispose(true);
            OnDisposed(new CoreDisposableEventArgs(_identifier));
            GC.SuppressFinalize(this);
        }

        protected virtual void OnDisposed(CoreDisposableEventArgs e)
        {
            DisposedEvt?.Invoke(this, e);
        }

        /// <summary>
        /// If Dispose already performed
        /// </summary>
        public bool Disposed { get; protected set; }

        public event EventHandler<CoreDisposableEventArgs> DisposedEvt;
    }

    internal class CoreDisposableInstance : CoreDisposable
    {
        /// <summary>
        /// Initialize a new instance of <see cref="CoreDisposable"/>
        /// </summary>
        internal CoreDisposableInstance(IIdentifier identifier) : base(identifier)
        {
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected override void Dispose(bool disposing)
        {
            //
        }
    }
}
