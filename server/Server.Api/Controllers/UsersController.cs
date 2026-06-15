using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Service.Interfaces;

namespace Server.Api.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
public class UsersController() : Controller
{
    [HttpPatch("{id:int}")]
    public async Task<IActionResult> UploadProfile([FromBody] Stream fileData, [FromRoute] int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (!Guid.TryParse(userIdClaim, out var userId))
        {
            return Unauthorized();
        }
        
        return Ok();
    }
}