using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using JetBrains.Annotations;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.EntityFramework.Audit
{
    internal  class EntityStatus
    {
        [NotMapped]
        public bool Empty => Entries.Count == 0;


        /// <summary>
        /// Uid of creation
        /// </summary>
        [StringLength(128)]
        public string TransactionId { get; set; }

        /// <summary>
        /// Reference to Create <see cref="TransactionAudit"/>
        /// </summary>
        [ForeignKey("TransactionId")]
        public virtual TransactionAudit TransactionAudit { get; set; }

        public virtual ICollection<EntryStatus> Entries { get; set; }

        public EntityStatus()
        {
            this.Entries = new HashSet<EntryStatus>();
        }

        
        internal void AddEntry([NotNull] EntryStatus e)
        {
            this.Entries.Add(e);
        }

        internal void AddRange([NotNull] IEnumerable<EntryStatus> l)
        {
            if (!l.Any())
            {
                return;
            }

            var en =l.OrderBy(x => x.Name).ToArray();
            foreach (var entryStatuse in en)
            {
                Entries.Add(entryStatuse);
            }

        }
       
       
        
    }
}