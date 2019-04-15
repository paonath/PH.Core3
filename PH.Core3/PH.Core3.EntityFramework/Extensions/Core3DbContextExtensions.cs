//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Runtime.CompilerServices;
//using System.Threading;
//using System.Threading.Tasks;
//using JetBrains.Annotations;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using PH.Core3.Common;
//using PH.Core3.Common.Models.Entities;
//using PH.Core3.EntityFramework.Audit;

//namespace PH.Core3.EntityFramework
//{
//    /// <summary>
//    /// Extensions methods allowing to perform Core3 Crud Ops and Auditing on DbContext
//    /// </summary>
//    internal static class Core3DbContextExtensions
//    {
//        //public static    int SaveBaseChangesWithAuthorAndTenant([NotNull] this DbContext context,[NotNull] IIdentifier identifier,[NotNull] string author, [NotNull] string TenantId)
//        //{
//        //    if (identifier is null) 
//        //        throw new ArgumentNullException(nameof(identifier));

//        //    if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
//        //        throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

//        //    var auditEntries = OnBeforeSaveChangesWithAuthorAndTenant(context,identifier,author,TenantId);
//        //    var result       = context.SaveChanges();
//        //    var auditsNum    = OnAfterSaveChangesWithAuthorAndTenant(context,auditEntries);
//        //    return result;
//        //}

//        //public static    int SaveBaseChangesWithAuthorAndTenant([NotNull] this DbContext context,[NotNull] IIdentifier identifier,[NotNull] string author, [NotNull] string tenantId, bool b)
//        //{
//        //    if (identifier is null) 
//        //        throw new ArgumentNullException(nameof(identifier));

//        //    if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
//        //        throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

//        //    var auditEntries = OnBeforeSaveChangesWithAuthorAndTenant(context,identifier,author,tenantId);
//        //    var result       = context.SaveChanges(b);
//        //    var auditsNum    = OnAfterSaveChangesWithAuthorAndTenant(context,auditEntries);
//        //    return result;
//        //}



//        public static  async Task<int> SaveBaseChangesWithAuthorAndTenantAsync([NotNull] this DbContext context,[NotNull] IIdentifier identifier,[NotNull] string author, [NotNull] string tenantId,CancellationToken cancellationToken =
//                                                            new CancellationToken())
//        {
//            if (identifier is null) throw new ArgumentNullException(nameof(identifier));
//            if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
//                throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

//            var auditEntries = OnBeforeSaveChangesWithAuthorAndTenant(context,identifier,author, tenantId);
//            var result       = await context.SaveChangesAsync(cancellationToken);
//            var auditsNum    = OnAfterSaveChangesWithAuthorAndTenant(context, auditEntries);
//            return result;
//        }

//        public static  async Task<int> SaveBaseChangesWithAuthorAndTenantAsync([NotNull] this DbContext context,[NotNull] IIdentifier identifier,[NotNull] string author
//                                                            , bool b, [NotNull] string tenantId,CancellationToken cancellationToken =
//                                                            new CancellationToken())
//        {
//            if (identifier is null) throw new ArgumentNullException(nameof(identifier));
//            if (string.IsNullOrEmpty(author) || string.IsNullOrWhiteSpace(author))
//                throw new ArgumentNullException(nameof(author), @"Author must be set before any savechanges");

//            var auditEntries =  OnBeforeSaveChangesWithAuthorAndTenant(context,identifier,author,tenantId);
//            var result       = await context.SaveChangesAsync(b, cancellationToken);
//            var auditsNum    = OnAfterSaveChangesWithAuthorAndTenantAsync(context,auditEntries);
//            return result;
//        }


//        [NotNull]
//        public static List<AuditEntry> OnBeforeSaveChangesWithAuthorAndTenant([NotNull] this DbContext context,[NotNull] IIdentifier identifier,[CanBeNull] string author, [NotNull] string tenantId)
//        {
//            context.ChangeTracker.DetectChanges();
            
//            var auditEntries = new List<AuditEntry>();

//            var transactionId = identifier.Uid;

//            foreach (var entry in context.ChangeTracker.Entries())
//            {
//                if (entry.Entity is Audit.Audit || entry.Entity is TransactionAudit ||
//                    entry.State == EntityState.Detached ||
//                    entry.State == EntityState.Unchanged)
//                {
//                    continue;
//                }

//                var e = entry.Entity as IEntity;
//                if (null != e)
//                {
//                    if (null == e.TenantId)
//                    {
//                        e.TenantId = tenantId;
//                        e.UpdatedTransactionId = identifier.Uid;

//                        if (entry.State == EntityState.Added)
//                        {
//                            if (string.IsNullOrEmpty(e.CreatedTransactionId))
//                                e.CreatedTransactionId = identifier.Uid;

//                            e.Deleted              = false;
//                        }

//                        if (entry.State == EntityState.Modified)
//                        {

//                        }
//                    }

                    
                    
//                }


//                var auditEntry = new AuditEntry(entry, transactionId, author)
//                {
//                    TableName = entry.Metadata.Relational().TableName
//                };


//                auditEntries.Add(auditEntry);

//                foreach (var property in entry.Properties)
//                {
//                    if (property.IsTemporary)
//                    {
//                        // value will be generated by the database, get the value after saving
//                        auditEntry.TemporaryProperties.Add(property);
//                        continue;
//                    }

//                    string propertyName = property.Metadata.Name;
//                    if (property.Metadata.IsPrimaryKey())
//                    {
//                        auditEntry.KeyValues[propertyName] = property.CurrentValue;
//                        continue;
//                    }

//                    switch (entry.State)
//                    {
//                        case EntityState.Added:
//                            auditEntry.NewValues[propertyName] = property.CurrentValue;
//                            break;

//                        case EntityState.Deleted:
//                            auditEntry.OldValues[propertyName] = property.OriginalValue;
//                            break;

//                        case EntityState.Modified:
//                            if (property.IsModified)
//                            {
//                                auditEntry.OldValues[propertyName] = property.OriginalValue;
//                                auditEntry.NewValues[propertyName] = property.CurrentValue;
//                            }

//                            break;
//                    }
//                }
//            }

//            // Save audit entities that have all the modifications
//            foreach (var auditEntry in auditEntries.Where(_ => !_.HasTemporaryProperties))
//            {
//                context.Set<Audit.Audit>().Add(auditEntry.ToAudit());
//                //Audits.Add(auditEntry.ToAudit());
//            }

//            // keep a list of entries where the value of some properties are unknown at this step
//            return auditEntries.Where(_ => _.HasTemporaryProperties).ToList();
//        }

//        public static  int OnAfterSaveChangesWithAuthorAndTenant(this DbContext context,[CanBeNull] List<AuditEntry> auditEntries)
//        {
//            if (auditEntries == null || auditEntries.Count == 0)
//                return 0;


//            foreach (var auditEntry in auditEntries)
//            {
//                // Get the final value of the temporary properties
//                foreach (var prop in auditEntry.TemporaryProperties)
//                {
//                    if (prop.Metadata.IsPrimaryKey())
//                    {
//                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
//                    }
//                    else
//                    {
//                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
//                    }
//                }

//                // Save the Audit entry

//                //Audits.Add(auditEntry.ToAudit());
//                context.Set<Audit.Audit>().Add(auditEntry.ToAudit());
//            }

//            return context.SaveChanges();
//        }

//        [NotNull]
//        public static async Task<int> OnAfterSaveChangesWithAuthorAndTenantAsync(this DbContext context,[CanBeNull] List<AuditEntry> auditEntries)
//        {
//            if (auditEntries == null || auditEntries.Count == 0)
//                return 0;



//            foreach (var auditEntry in auditEntries)
//            {
//                // Get the final value of the temporary properties
//                foreach (var prop in auditEntry.TemporaryProperties)
//                {
//                    if (prop.Metadata.IsPrimaryKey())
//                    {
//                        auditEntry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
//                    }
//                    else
//                    {
//                        auditEntry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
//                    }
//                }

//                // Save the Audit entry
//                //Audits.Add(auditEntry.ToAudit());
//                await context.Set<Audit.Audit>().AddAsync(auditEntry.ToAudit());
//            }

//            return await context.SaveChangesAsync();
//        }



//        //public static void AssignTenantAndOtherQueryFilterOnModelCreatingByExt(this DbContext context, [NotNull] ModelBuilder builder, [NotNull] string tenantId, [NotNull] Type[] entityTypes)
//        //{
//        //     foreach (Type entityType in entityTypes)
//        //    {
//        //        #region QueryFilters

//        //        var entityName     = entityType.Name;
//        //        var entityFullName = entityType?.FullName ?? entityName;
//        //        var paramName      = $"{entityName}_p";

//        //        var entityTypeFromModel = builder.Model.FindEntityType(entityFullName);
//        //        var queryFilter         = entityTypeFromModel.QueryFilter;
//        //        var queryParam          = queryFilter?.Parameters.FirstOrDefault();
//        //        if (null != queryParam)
//        //        {
//        //            paramName = queryParam.Name;
//        //        }


//        //        ParameterExpression paramExpr = Expression.Parameter(entityType, paramName);
//        //        Expression bodyTenant = Expression.Equal(Expression.Property(paramExpr, "TenantId"),
//        //                                                 Expression.Constant(tenantId)
//        //                                                );
//        //        Expression bodyDeleted = Expression.Equal(Expression.Property(paramExpr, "Deleted"),
//        //                                                  Expression.Constant(false)
//        //                                                 );
//        //        Expression body = null;
//        //        if (null != queryFilter)
//        //        {
//        //            var res = Expression.Lambda(Expression.Invoke(queryFilter, paramExpr), paramExpr);


//        //            body = Expression.AndAlso(Expression.AndAlso(bodyDeleted, bodyTenant), res.Body);
//        //        }
//        //        else
//        //        {
//        //            body = Expression.AndAlso(bodyDeleted, bodyTenant);
//        //        }


//        //        var name = $"TenantQueryFilter_{entityName}";

//        //        LambdaExpression lambdaExpr = Expression.Lambda(body, name,
//        //                                                        new List<ParameterExpression>() {paramExpr}
//        //                                                       );

//        //        var entyTYpeBuilder = builder.Entity(entityType);


//        //        entyTYpeBuilder.HasQueryFilter(lambdaExpr);

//        //        #endregion
//        //    }
//        //}

//    }
//}