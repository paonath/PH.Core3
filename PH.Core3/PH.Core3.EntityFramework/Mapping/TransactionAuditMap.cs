using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PH.Core3.Common.Models.Entities;

namespace PH.Core3.EntityFramework.Mapping
{
    internal class TransactionAuditMap : IEntityTypeConfiguration<TransactionAudit>
    {
        /// <inheritdoc />
        public void Configure([NotNull] EntityTypeBuilder<TransactionAudit> builder)
        {
            builder.ToTable("transaction_audit");
            
            builder.Property(x => x.MillisecDuration).HasDefaultValue(0);
            builder.Property(x => x.Progr).HasDefaultValue(1);


            builder
                .HasIndex(i => new
                {
                    i.Id,
                    i.Author,
                    i.UtcDateTime,
                    i.Timestamp,
                    i.TenantId
                });
        }
    }
}