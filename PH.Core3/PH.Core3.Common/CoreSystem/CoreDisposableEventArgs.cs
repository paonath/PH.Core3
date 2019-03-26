namespace PH.Core3.Common.CoreSystem
{
    /// <summary>
    /// Event Args for Disposed CoreDisposable
    /// </summary>
    public class CoreDisposableEventArgs : CoreEventArgs
    {
        public CoreDisposableEventArgs(IIdentifier identifier) 
            : base(identifier)
        {
        }
    }
}