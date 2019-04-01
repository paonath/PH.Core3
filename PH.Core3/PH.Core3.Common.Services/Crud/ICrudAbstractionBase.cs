using System;
using JetBrains.Annotations;

namespace PH.Core3.Common.Services.Crud
{
    /// <summary>
    /// Base Service for Organize Crud Services
    /// </summary>
    public interface ICrudAbstractionBase
    {

        /// <summary>
        /// Init a Scope in which validation is not performed.
        /// </summary>
        /// <param name="scopeName">Optional Scope Name</param>
        /// <returns>Disposable Scope</returns>
        IDisposable BeginNoValidationScope([CanBeNull] string scopeName = "");
    }
}