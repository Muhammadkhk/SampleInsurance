
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Sample.API.Controllers
{
    [ApiController]
    //[Consumes(MediaTypeNames.Application.Json)]
    //[Produces(MediaTypeNames.Application.Json)]
    [Route("[controller]")]
    public class BaseController : Controller
    {
    }
}
