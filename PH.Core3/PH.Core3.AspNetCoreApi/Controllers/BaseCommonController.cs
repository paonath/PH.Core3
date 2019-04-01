using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PH.Core3.UnitOfWork;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]
namespace PH.Core3.AspNetCoreApi.Controllers
{
    public abstract class BaseCommonController: Controller
    {
        protected readonly IUnitOfWork Uow;

        /// <summary>
        /// True if Already perfomed a Commit on <see cref="IUnitOfWork"/>
        /// </summary>
        protected bool AlreadyCommitted;

        /// <summary>
        /// Log Service
        /// </summary>
        protected readonly ILogger<BaseCommonController> Logger;

        public BaseCommonController(IUnitOfWork uow, ILogger<BaseCommonController> logger)
        {
            Uow = uow;
            Logger = logger;
        }

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

        protected virtual void Rollback()
        {
            AlreadyCommitted = false;
            Uow.Rollback();
        }
    }
}