using System.Collections.Generic;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PH.Core3.Common;
using PH.Core3.Common.Result;
using PH.Core3.Test.WebApp.Services;
using PH.Core3.UnitOfWork;

namespace PH.Core3.Test.WebApp.Areas.v1.Controllers
{

    

    /// <summary>
    /// Controller V1
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiVersion( "1" )]
    //[Route( "api/v{version:apiVersion}/[controller]" )]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ILogger<ValuesController> _logger;
        private readonly AlberoService _alberoService;
        private readonly IIdentifier _identifier;
        private readonly IUnitOfWork _uow;

        public ValuesController(IIdentifier identifier, ILogger<ValuesController> logger, IUnitOfWork uow, AlberoService alberoService)
        {
            _identifier = identifier;
            _logger = logger;
            _uow = uow;
            _alberoService = alberoService;

            _logger.BeginScope("test scope");
            _logger.LogInformation("Passo di qui");
        }


        // GET api/values
        [HttpGet]
        [NotNull]
        public ActionResult<IEnumerable<string>> Get()
        {
            var tst = _alberoService.LoadAllAsync();
            tst.Wait();
            var r = tst.Result;

            

        

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        [NotNull]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
