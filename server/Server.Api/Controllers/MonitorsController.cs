using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Extensions;
using Server.Domain.Dto.Request;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Interfaces.Service;

namespace Server.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class MonitorsController(IMonitorService monitorService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await monitorService.GetAll(User.GetUserId()));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MonitorCreateRequest request)
    {
        return Ok(await monitorService.Create(request, User.GetUserId()));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update([FromBody] MonitorUpdateRequest request, [FromRoute] Guid id)
    {
        await monitorService.Update(request, id);

        return Ok();
    }
}