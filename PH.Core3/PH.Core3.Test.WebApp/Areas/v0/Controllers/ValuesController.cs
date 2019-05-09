using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using PH.Core3.Test.WebApp.HostedService;

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
        private readonly IMailSenderService _mailSenderService;

        public ValuesController(IMailSenderService mailSenderService)
        {
            _mailSenderService = mailSenderService;
        }


        // GET api/values
        [HttpGet]
        [ItemNotNull]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            await _mailSenderService.SendEmailAsync("pippo@gmail.com");

            return new string[] { "value0", "value28" };
           
        }


    }

    public class VValidator : FluentValidation.AbstractValidator<string>
    {
        public VValidator()
        {
            //RuleFor().
        }
    }
}