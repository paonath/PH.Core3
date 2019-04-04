using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH.Core3.Common.Models.Entities
{
    /// <summary>
    /// Entity Base Class to Persist on Db
    ///
    /// All entities that need to persist on the database must implement the current interface
    /// </summary>
    /// <typeparam name="TKey">Type of the Id property</typeparam>
    public abstract class Entity<TKey> : IEntity<TKey>, IEntity
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Unique Id of current class
        /// </summary>
        [Required]
        public TKey Id { get; set; }


        /// <summary>
        /// True if Entity is Deleted
        ///
        /// (Logical delete)
        /// </summary>
        [Required]
        public bool Deleted { get; set; }



        /// <summary>
        /// Tenant Identifier
        /// </summary>
        [Required]
        [StringLength(128)]
        public string TenantId { get; set; }


        /// <summary>
        ///  Uid If <see cref="IEntity.Deleted"/> 
        /// </summary>
        [StringLength(128)]
        public string DeletedTransactionId { get; set; }

        /// <summary>
        /// Reference to Delete <see cref="TransactionAudit"/> 
        /// </summary>
        [ForeignKey("DeletedTransactionId")]
        public virtual TransactionAudit DeletedTransaction { get; set; }

        /// <summary>
        /// Uid of creation
        /// </summary>
        [StringLength(128)]
        public string CreatedTransactionId { get; set; }

        /// <summary>
        /// Reference to Create <see cref="TransactionAudit"/>
        /// </summary>
        [ForeignKey("CreatedTransactionId")]
        public virtual TransactionAudit CreatedTransaction { get; set; }

        /// <summary>
        /// Uid of update
        /// </summary>
        [StringLength(128)]
        public string UpdatedTransactionId { get; set; }

        /// <summary>
        /// Reference to Update <see cref="TransactionAudit"/>
        /// </summary>
        [ForeignKey("UpdatedTransactionId")]
        public virtual TransactionAudit UpdatedTransaction { get; set; }


        /// <summary>
        /// Row Version and Concurrency Check Token
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }

       
    }
}