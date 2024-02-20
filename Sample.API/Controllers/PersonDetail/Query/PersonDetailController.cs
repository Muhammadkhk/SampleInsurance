using Sample.Framework.ApplicationService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Sample.ApplicationService.PersonDetail.Query;
using Sample.API.Controllers;
using Sample.Core.Infrastructure;
using Sample.API;
public class PersonDetailController : BaseController
 {
        private readonly SampleDbContext _connection;
        public PersonDetailController(SampleDbContext connection)
        {
            _connection = connection;
        }

        [HttpGet]
        [ProducesResponseType(typeof(QueryResult<PersonDetailReadContracts.GetAllPersonDetailsByPagination>), StatusCodes.Status200OK)]
        public Task<IActionResult> GetAllDevicesByPagination([FromQuery] PersonDetailQueryContracts.GetAllPersonDetailsByPagination request, CancellationToken token)
        => RequestHandler.HandleQuery(request, _connection.Query);


}

