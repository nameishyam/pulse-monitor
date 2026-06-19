using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Domain.Interfaces.Service;

namespace Server.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class LogsController(ILogService logService) : ControllerBase
{
    [HttpGet("{monitorId:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid monitorId)
    {
        return Ok(await logService.GetByMonitor(monitorId));
    }
}