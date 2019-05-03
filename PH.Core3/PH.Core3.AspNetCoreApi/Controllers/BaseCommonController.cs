using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PH.Core3.UnitOfWork;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace PH.Core3.AspNetCoreApi.Controllers
{
    /// <summary>
    /// Base Common Controller
    /// </summary>
    public abstract class BaseCommonController: Controller
    {
        /// <summary>
        /// Unit Of Work
        /// </summary>
        protected readonly IUnitOfWork Uow;

        /// <summary>
        /// True if Already perfomed a Commit on <see cref="IUnitOfWork"/>
        /// </summary>
        protected bool AlreadyCommitted;

        /// <summary>
        /// Log Service
        /// </summary>
        protected readonly ILogger<BaseCommonController> Logger;

        /// <summary>
        /// Init new instance of controller
        /// </summary>
        /// <param name="uow">unit of work</param>
        /// <param name="logger">logger</param>
        public BaseCommonController(IUnitOfWork uow, ILogger<BaseCommonController> logger)
        {
            Uow = uow;
            Logger = logger;
        }

        /// <summary>
        /// Commit
        /// </summary>
        /// <param name="commitMessage">commit message</param>
        protected virtual void Commit(string commitMessage = "")
        {
            if(AlreadyCommitted)
                return;
            
            try
            {
                Uow.Commit(commitMessage);
                AlreadyCommitted = true;
            }
            catch (Exception e)
            {
                Logger.LogCritical(e, $"ErrorAsync commit transaction with uid {Uow.Identifier.Uid}");
                Rollback();
                throw;
            }
        }

        /// <summary>
        /// Rollback
        /// </summary>
        protected virtual void Rollback()
        {
            AlreadyCommitted = false;
            Uow.Rollback();
        }
    }
}