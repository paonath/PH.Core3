using System;
using JetBrains.Annotations;

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
            return new NamedScope(name);
        }
        
        /// <summary>
        /// Scope Name
        /// </summary>
        public string Scope { get; private set; }

        private NamedScope([NotNull] string scopeName)
        {
            Scope    = scopeName ?? throw new ArgumentNullException(nameof(scopeName));
            Disposed = false;
        }

        /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
        public void Dispose()
        {
            Disposed = true;
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// True if Disposed Scope
        /// </summary>
        public bool Disposed { get; private set; }
    }
}