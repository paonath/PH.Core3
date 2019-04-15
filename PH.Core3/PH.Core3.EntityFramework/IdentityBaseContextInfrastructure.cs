using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PH.Core3.Common;
using PH.Core3.Common.Models.Entities;
using PH.Core3.EntityFramework.Audit;

namespace PH.Core3.EntityFramework
{
    public abstract class IdentityBaseContextInfrastructure<TUser, TRole, TKey> : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<TUser, TRole, TKey>
        where TUser : IdentityUser<TKey>, IEntity<TKey>

        where TRole : IdentityRole<TKey>, IEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        internal DbSet<Audit.Audit> Audits { get; set; }

        protected IdentityBaseContextInfrastructure([NotNull] DbContextOptions options)
            :base(options)
        {
            Changecount = 0;
        }

        /// <summary>
        /// Tenant Identifier
        /// </summary>
        public string TenantId { get; set; }

        public int Changecount { get; protected set; }



        protected  int SaveBaseChanges([NotNull] IIdentifier identifier,[NotNull] string author)
        {
            if (identifier is null)
                throw new ArgumentNullException(nameof(identifier));

            if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
                throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

            var auditEntries = this.OnBeforeSaveChanges(identifier, author);
            var result = base.SaveChanges();
            var auditsNum = OnAfterSaveChanges(auditEntries);
            Changecount += result;
            return result;

            
        }

        protected  int SaveBaseChanges([NotNull] IIdentifier identifier,[NotNull] string author, bool b)
        {
            if (identifier is null)
                throw new ArgumentNullException(nameof(identifier));

            if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
                throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

            var auditEntries = OnBeforeSaveChanges(identifier, author);
            var result = base.SaveChanges(b);
            var auditsNum = OnAfterSaveChanges(auditEntries);
            Changecount += result;
            return result;

            

        }



        protected  async Task<int> SaveBaseChangesAsync([NotNull] IIdentifier identifier,[NotNull] string author,CancellationToken cancellationToken =
                                                            new CancellationToken())
        {
            if (identifier is null) throw new ArgumentNullException(nameof(identifier));
            if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
                throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

            var auditEntries = OnBeforeSaveChanges(identifier, author);
            var result = await base.SaveChangesAsync(cancellationToken);
            var auditsNum = await OnAfterSaveChangesAsync(auditEntries);
            Changecount += result;
            return result;

            
        }

        protected  async Task<int> SaveBaseChangesAsync([NotNull] IIdentifier identifier,[NotNull] string author, bool b,CancellationToken cancellationToken =
                                                            new CancellationToken())
        {
            if (identifier is null) throw new ArgumentNullException(nameof(identifier));
            if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
                throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

            var auditEntries = OnBeforeSaveChanges(identifier, author);
            var result = await base.SaveChangesAsync(b, cancellationToken);
            var auditsNum = await OnAfterSaveChangesAsync(auditEntries);
            Changecount += result;
            return result;

            
        }

        #region Audits






        [NotNull]
        private List<AuditEntry> OnBeforeSaveChanges(IIdentifier identifier, string author)
        {
            ChangeTracker.DetectChanges();
            var auditEntries = new List<AuditEntry>();

            var transactionId = identifier.Uid;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Audit.Audit || entry.Entity is TransactionAudit ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                if (entry.Entity is IEntity e)
                {
                    if (null == e.TenantId)
                    {
                        e.TenantId = TenantId;

                        if (entry.State == EntityState.Added)
                        {
                            e.CreatedTransactionId = identifier.Uid;
                            e.Deleted = false;
                        }

                        if (entry.State == EntityState.Modified)
                        {
                            if (string.IsNullOrEmpty(e.UpdatedTransactionId))
                                e.UpdatedTransactionId = identifier.Uid;

                        }
                    }



                }




                var auditEntry = new AuditEntry(entry, transactionId, author)
                {
                    TableName = entry.Metadata.Relational().TableName
                };


                auditEntries.Add(auditEntry);

                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        // value will be generated by the database, get the value after saving
                        auditEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    string propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditEntry.NewValues[propertyName] = property.CurrentValue;
                            break;

                        case EntityState.Deleted:
                            auditEntry.OldValues[propertyName] = property.OriginalValue;
                            break;

                        case EntityState.Modified:
                            if (property.IsModified)
                            {
                                auditEntry.OldValues[propertyName] = property.OriginalValue;
                                auditEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            // Save audit entities that have all the modifications
            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
            {
                Audits.Add(auditEntry.ToAudit());
            }

            // keep a list of entries where the value of some properties are unknown at this step
            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
        }


       

        private int OnAfterSaveChanges([CanBeNull] List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return 0;


            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry

                Audits.Add(auditEntry.ToAudit());
            }

            return SaveChanges();
        }

        [NotNull]
        private async Task<int> OnAfterSaveChangesAsync([CanBeNull] List<AuditEntry> auditEntries)
        {
            if (auditEntries == null || auditEntries.Count == 0)
                return 0;



            foreach (var auditEntry in auditEntries)
            {
                // Get the final value of the temporary properties
                foreach (var prop in auditEntry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                    else
                    {
                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }

                // Save the Audit entry
                Audits.Add(auditEntry.ToAudit());
            }

            return await base.SaveChangesAsync();
        }

        #endregion

    }
}