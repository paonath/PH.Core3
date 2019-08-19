using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
{
    /// <summary>
    /// Entity Base Interface to Persist on Db
    ///
    /// All entities that need to persist on the database must implement the current interface
    /// </summary>
    public interface IEntity 
    {
        /// <summary>
        /// Row Version and Concurrency Check Token
        /// </summary>
        [Timestamp]
        byte[] Timestamp { get; set; }

        /// <summary>
        /// True if Entity is Deleted
        ///
        /// (Logical delete)
        /// </summary>
        [Required]
        bool Deleted { get; set; }

        /// <summary>
        /// Tenant Identifier
        /// </summary>
        [Required]
        [StringLength(128)]
        string TenantId { get; set; }

        /// <summary>
        ///  Uid If <see cref="Deleted"/> 
        /// </summary>
        [StringLength(128)]
        string DeletedTransactionId { get; set; }

        /// <summary>
        /// Reference to Delete <see cref="TransactionAudit"/> 
        /// </summary>
        [ForeignKey("DeletedTransactionId")]
        TransactionAudit DeletedTransaction { get; set; }
        


        /// <summary>
        /// Uid of creation
        /// </summary>
        [StringLength(128)]
        string CreatedTransactionId { get; set; }

        /// <summary>
        /// Reference to Create <see cref="TransactionAudit"/>
        /// </summary>
        [ForeignKey("CreatedTransactionId")]
        TransactionAudit CreatedTransaction { get; set; }

        /// <summary>
        /// Uid of update
        /// </summary>
        [StringLength(128)]
        string UpdatedTransactionId { get; set; }

        /// <summary>
        /// Reference to Update <see cref="TransactionAudit"/>
        /// </summary>
        [ForeignKey("UpdatedTransactionId")]
        TransactionAudit UpdatedTransaction { get; set; }

    }

    /// <summary>
    /// Entity Base Interface to Persist on Db
    ///
    /// All entities that need to persist on the database must implement the current interface
    /// </summary>
    /// <typeparam name="TKey">Type of the Id property</typeparam>
    public interface IEntity<TKey> : IEntity 
        where TKey : IEquatable<TKey>
    {
        
    }
}