using System;

namespace PH.Core3.Common.Bus
{
    /// <summary>
    /// Action Bus 
    /// </summary>
    public interface IActionBus
    {
        /// <summary>Gets the count of queued actions.</summary>
        /// <value>The count.</value>
        int Count { get; }

        /// <summary>Enqueues the specified action.</summary>
        /// <param name="action">The action.</param>
        void Enqueue(Action action);

        /// <summary>Flush all actions.</summary>
        /// <param name="throwExceptionOnError">if set to <c>true</c>throw exception on error.</param>
        void Flush(bool throwExceptionOnError = false);
    }
}