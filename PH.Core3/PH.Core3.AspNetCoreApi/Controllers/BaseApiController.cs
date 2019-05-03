using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PH.Core3.Common.Result;
using PH.Core3.UnitOfWork;

namespace PH.Core3.AspNetCoreApi.Controllers
{
    /// <summary>
    /// Base Api Controller
    /// </summary>
    public abstract class BaseApiController : BaseCommonController
    {
        /// <summary>
        /// Init new instance of controller
        /// </summary>
        /// <param name="uow">unit of work</param>
        /// <param name="logger">logger</param>
        protected BaseApiController(IUnitOfWork uow, ILogger<BaseCommonController> logger) : base(uow, logger)
        {
        }


        /// <summary>
        ///  Return Error to client
        /// </summary>
        /// <param name="content">Result</param>
        /// <typeparam name="TContent">Type of Result</typeparam>
        /// <returns>Http 500 Error</returns>
        protected virtual async Task<ActionResult<TContent>> ErrorTypedAsync<TContent>(IResult<TContent> content)
        {
            return await Task.FromResult(StatusCode(StatusCodes.Status500InternalServerError, content.Errors));
        }

        /// <summary>
        ///  Perform a Typed Action
        /// </summary>
        /// <typeparam name="TContent">Type of Result</typeparam>
        /// <param name="action">Action to perform</param>
        /// <returns>200 if Ok</returns>
        protected virtual async Task<ActionResult<TContent>> PerformAsync<TContent>(Func<Task<IResult<TContent>>> action)
        {
            if (null == action)
                throw new ArgumentNullException(nameof(action), "Action not provided!");

            try
            {
                if (!ModelState.IsValid)
                    return await Task.FromResult(BadRequest(ModelState)) ;


                var result = await action.Invoke();


                if (result.OnError)
                {
                    Rollback();
                    return await ErrorTypedAsync(result);
                }

                
                Commit();

                if (null == result.Content)
                    return NotFound();

                return Ok(result.Content);
            }
            catch (Exception e)
            {
                Logger.LogCritical(e,e.Message);
                var errResult = ResultFactory.Fail(Uow.Identifier , e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, errResult.Errors);
            }

        }
    }
}