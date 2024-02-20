using Sample.ApplicationService.PersonDetail.Command;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;
using Sample.ApplicationService.PersonDetailCommand.Command;
using Sample.API;

namespace CoreInspect.Core.API.Controllers.PersonDetail.Command
{
    [ApiController]
    [Route("[controller]")]
    public class PersonDetailController : Controller
    {
        private readonly PersonDetailCommandApplicationService _applicationService;
        public PersonDetailController(PersonDetailCommandApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(PersonDetailCommandContracts.Create request, CancellationToken token)
        => await RequestHandler.HandleCommand(request, _applicationService.Handle);

    }
}
