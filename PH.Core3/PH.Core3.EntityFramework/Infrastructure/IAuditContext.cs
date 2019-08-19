using Microsoft.EntityFrameworkCore;
using PH.Core3.EntityFramework.Abstractions.Models.Entities;

namespace PH.Core3.EntityFramework.Infrastructure
{
    internal interface IAuditContext
    {
        DbSet<TransactionAudit> TransactionAudits { get; set; }
    }
}