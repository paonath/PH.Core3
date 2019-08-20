using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PH.Core3.EntityFramework.Abstractions.Models.Entities
{
    /// <summary>
    /// Class for Tracking Audits on Db.
    /// </summary>
    public class TransactionAudit 
    {
        /// <summary>
        /// Unique Id of current transaction
        /// </summary>
        public long Id { get; set; }

        /// <summary>Gets or sets the string identifier: unique, assigned.</summary>
        /// <value>The string identifier.</value>
        [StringLength(128)]
        public string StrIdentifier { get; set; }

        /// <summary>
        /// Author of current Transaction
        /// </summary>
        [StringLength(500)]
        public string Author { get; set; }


        /// <summary>
        /// Date And Time UTC of Current Transaction Open
        /// </summary>
        public DateTime UtcDateTime { get; set; }


        /// <summary>
        /// Row Version and Concurrency Check Token
        /// </summary>
        [Timestamp]
        public byte[] Timestamp { get; set; }

       
        /// <summary>
        /// Millises duration of current Transaction
        /// </summary>
        public double MillisecDuration { get; set; }

        /// <summary>
        /// Tenant Identifier
        /// </summary>
        [Required]
        public int TenantId { get; set; }

        /// <summary>Gets or sets the tenant.</summary>
        /// <value>The tenant.</value>
        [ForeignKey("TenantId")]
        public virtual Tenant Tenant { get; set; }

        /// <summary>
        /// Transaction Scopes
        /// </summary>
        [StringLength(500)]
        public string Scopes { get; set; }

        /// <summary>
        /// A Custom LogMessage related to this transaction (set by IUnitOfWork)
        /// </summary>
        [StringLength(500)]
        public string CommitMessage { get; set; }
    }
}