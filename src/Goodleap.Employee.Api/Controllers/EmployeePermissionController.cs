using Goodleap.Employee.Api.Business.Commands.EmployeePermissions;
using Goodleap.Employee.Api.Business.Queries.EmployeePermissions;
using Goodleap.Employee.Api.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Goodleap.Employee.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeePermissionController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EmployeePermissionController> _logger;
        public EmployeePermissionController(IMediator mediator, ILogger<EmployeePermissionController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPatch("update")]
        public async Task<IActionResult> UpdateEmployeePermission([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            _logger.LogInformation("Updating permissions");
            return Ok(await _mediator.Send(new UpdatePermissionCommand(updatePermissionDto)));
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllEmployeePermission()
        {
            _logger.LogInformation("Retrieving Information");
            return Ok(await _mediator.Send(new GetAllEmployeePermissionQuery()));
        }
    }
}
