namespace PH.Core3.Common.CoreSystem
{
    /// <summary>
    /// Event Args for Disposed CoreDisposable
    /// </summary>
    public class CoreDisposableEventArgs : CoreEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CoreDisposableEventArgs"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        public CoreDisposableEventArgs(IIdentifier identifier) 
            : base(identifier)
        {
        }
    }
}