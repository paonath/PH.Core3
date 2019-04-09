using Microsoft.Extensions.Logging;
using PH.Core3.UnitOfWork;

namespace PH.Core3.EntityFramework
{
    public interface IDbContextUnitOfWork : IUnitOfWork
    {
        ILogger UowLogger { get; set; }

        bool Initialized { get; }
        IDbContextUnitOfWork Initialize();
    }
}