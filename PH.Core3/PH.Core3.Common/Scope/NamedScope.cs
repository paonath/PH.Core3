using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;

namespace PH.Core3.Common.Scope
{
    /// <summary>
    /// A Named <see cref="IDisposable">Scope</see>
    /// </summary>
    public class NamedScope : IDisposable
    {

        /// <summary>
        /// Initialize a new instance of a <see cref="NamedScope"/>
        /// </summary>
        /// <param name="name">Scope Name</param>
        /// <returns><see cref="IDisposable"/> scope</returns>
        [NotNull]
        public static NamedScope Instance([NotNull] string name)
        {
            return Instance(name, null);
        }

        /// <summary>Initialize a new instance of a <see cref="NamedScope"/></summary>
        /// <param name="name">The name.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>IDisposable scope</returns>
        [NotNull]
        public static NamedScope Instance([NotNull] string name, ILogger logger)
        {
            return new NamedScope(name, logger);
        }
        
        /// <summary>
        /// Scope Name
        /// </summary>
        public string Scope { get; private set; }

        private IDisposable _logScope;

        private NamedScope([NotNull] string scopeName, [CanBeNull] ILogger logger)
        {
            Scope    = scopeName ?? throw new ArgumentNullException(nameof(scopeName));
            Disposed = false;
            if (null != logger)
            {
                _logScope = logger.BeginScope(scopeName);
            }
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disposed = true;
            _logScope?.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// True if Disposed Scope
        /// </summary>
        public bool Disposed { get; private set; }
    }
}