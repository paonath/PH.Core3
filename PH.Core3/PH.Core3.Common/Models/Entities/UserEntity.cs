using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace PH.Core3.Common.Models.Entities
{
    /// <summary>
    /// User entity
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUser{String}" />
    /// <seealso cref="PH.Core3.Common.Models.Entities.IEntity{String}" />
    public abstract class UserEntity : Microsoft.AspNetCore.Identity.IdentityUser<string> , IEntity<string>
    {
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
