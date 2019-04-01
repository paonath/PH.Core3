using System;
using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.CoreSystem;
using PH.Core3.Common.Models.Entities;
using PH.Core3.UnitOfWork;

namespace PH.Core3.EntityFramework
{
    public interface IIdentityBaseContext<TUser, TRole, TKey> : ITenantContext, IInitializable<IdentityBaseContext<TUser, TRole, TKey>>
        where TUser : IdentityUser<TKey>, IEntity<TKey> where TRole : IdentityRole<TKey>, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        ILogger Logger { get; set; }
        Dictionary<int, string> ScopeDictionary { get; }

        

        /// <summary>
        /// Identifier
        /// </summary>
        IIdentifier Identifier { get; set; }

        string Author { get; set; }

        /// <summary>
        /// Ctx Uid
        /// </summary>
        Guid CtxUid { get; }

        CancellationTokenSource CancellationTokenSource { get; set; }
        CancellationToken CancellationToken { get; }

        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        void OnCustomModelCreating([NotNull] ModelBuilder builder);

        IDisposable BeginScope([NotNull] string scopeName);


    }
}