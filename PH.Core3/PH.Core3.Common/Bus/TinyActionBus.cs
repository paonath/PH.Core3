using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Result;

namespace PH.Core3.Common.Bus
{
    /// <summary>
    /// Action Bus 
    /// </summary>
    /// <seealso cref="PH.Core3.Common.Bus.IActionBus" />
    public class TinyActionBus : IActionBus
    {
        private readonly Queue<Action> _actions;
        private readonly ILogger<TinyActionBus> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TinyActionBus"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public TinyActionBus(ILogger<TinyActionBus> logger)
        {
            _logger = logger;
            _actions = new Queue<Action>();
        }


        /// <summary>Gets the count of queued actions.</summary>
        /// <value>The count.</value>
        public int Count => _actions.Count;

        /// <summary>Enqueues the specified action.</summary>
        /// <param name="action">The action.</param>
        public void Enqueue(Action action)
        {
            _actions.Enqueue(action);
        }

        /// <summary>Flush all actions.</summary>
        /// <param name="throwExceptionOnError">if set to <c>true</c>[throw exception on error.</param>
        /// <exception cref="Exception">if action on error and throwExceptionOnError is set to true</exception>
        /// <returns></returns>
        public virtual void Flush(bool throwExceptionOnError = false)
        {
            
            while (_actions.Count > 0)
            {
                var act = _actions.Dequeue();
                try
                {
                    act.Invoke();
                }
                catch (Exception e)
                {
                    if (throwExceptionOnError)
                    {
                        _logger.LogCritical(e, $"Error performing action: {e.Message}");
                        throw;
                    }
                    else
                    {
                        _logger.LogError(e, $"Error performing action: {e.Message}");
                    }
                }
            }
        }
    }
}