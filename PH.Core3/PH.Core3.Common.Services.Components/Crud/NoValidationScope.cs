using System;
using JetBrains.Annotations;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.CoreSystem;

namespace PH.Core3.Common.Services.Components.Crud
{
    /// <summary>
    /// Event fired on Disposing a <see cref="NoValidationScope"/>
    /// </summary>
    public class NoValidationScopeDisposingEventArgs : CoreEventArgs
    {
        /// <summary>
        /// Init a No-ValidationScope Event Argument 
        /// </summary>
        /// <param name="identifier">Cross-Scope Identifier</param>
        public NoValidationScopeDisposingEventArgs([NotNull] IIdentifier identifier) : base(identifier)
        {
        }
    }

    /// <summary>
    /// Scope in which validation is not performed.
    /// 
    /// </summary>
    public class NoValidationScope : CoreDisposable
    {
        private readonly ILogger _logger;
        private IDisposable _disposable;
        private new readonly IIdentifier _identifier;


        /// <summary>
        /// Event fired before disposing no validation scope
        /// </summary>
        public EventHandler<NoValidationScopeDisposingEventArgs> DisposingNoValidationScope;

        /// <summary>
        /// Init Scope in which validation is not performed.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="scopeName">Scope Name</param>
        /// <exception cref="ArgumentNullException">On null Logger</exception>
        private NoValidationScope([NotNull] ILogger logger, [NotNull] IIdentifier identifier, [CanBeNull] string scopeName = "")
            :base(identifier)
        {
            _logger     = logger ?? throw new ArgumentNullException(nameof(logger));
            _identifier = identifier ?? throw new ArgumentNullException(nameof(identifier));
            var msg = "Begin a No-Validation Scope";

            if (!string.IsNullOrEmpty(scopeName))
            {
                msg = $"{msg} - Scope Name '{scopeName}'";
            }

            _disposable = _logger.BeginScope(scopeName);
        }

        /// <summary>
        /// Init Scope in which validation is not performed.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="identifier">Identifier</param>
        /// <param name="scopename">Optional Scope Name</param>
        /// <exception cref="ArgumentNullException">On null Logger</exception>
        /// <returns>NoValidationScope</returns>
        [NotNull]
        public static NoValidationScope Instance([NotNull] ILogger logger,[NotNull] IIdentifier identifier, [CanBeNull] string scopename = "") =>
            new NoValidationScope(logger, identifier, scopename);


        /// <summary>
        /// On Disposing should reset validation
        /// </summary>
        /// <param name="args"></param>
        protected virtual void OnDisposing(NoValidationScopeDisposingEventArgs args)
        {
            EventHandler<NoValidationScopeDisposingEventArgs> handler = DisposingNoValidationScope;
            handler?.Invoke(this, args);
        }

        /// <summary>
        /// Dispose Pattern.
        /// This method check if already <see cref="CoreDisposable.Disposed"/> (and set it to True).
        /// </summary>
        /// <param name="disposing">True if disposing</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !Disposed)
            {
                _logger.LogInformation("End No-Validation Scope");
                OnDisposing(new NoValidationScopeDisposingEventArgs(_identifier));

                _disposable.Dispose();
                Disposed = true;
                GC.SuppressFinalize(this);
            }

           
        }
    }
}