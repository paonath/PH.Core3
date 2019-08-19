﻿using System;
using System.ComponentModel.DataAnnotations;

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
        [StringLength(128)]
        public string Id { get; set; }

        /// <summary>
        /// Progressive order number
        /// </summary>
        public long Progr { get; set; }


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
        [StringLength(128)]
        public string TenantId { get; set; }

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