using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace PH.Core3.Test.WebApp.Areas.v0.Controllers
{
    /// <summary>
    /// Controller V0
    /// </summary>
    [Route("api/v0/[controller]")]
    [ApiVersion( "0" )]
    //[Route( "api/v{version:apiVersion}/[controller]" )]
    [ApiController]
    public class ValuesController : ControllerBase
    {


        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value0", "value28" };
        }


    }
}