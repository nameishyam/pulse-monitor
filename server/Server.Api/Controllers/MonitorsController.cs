using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Extensions;
using Server.Domain.Dto.Request.Create;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Interfaces.Service;
using Server.Service.Exceptions;

namespace Server.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class MonitorsController(IMonitorService monitorService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        try
        {
            return Ok(await monitorService.GetById(id));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MonitorCreate request)
    {
        return Ok(await monitorService.Create(request, User.GetUserId()));
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update([FromBody] MonitorUpdate request, [FromRoute] Guid id)
    {
        await monitorService.Update(request, id);

        return Ok();
    }
}