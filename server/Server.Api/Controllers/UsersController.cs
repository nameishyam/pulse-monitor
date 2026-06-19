using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Service.Exceptions;
using Server.Api.Extensions;
using Server.Domain.Dto.Request.Update;
using Server.Domain.Interfaces.Service;

namespace Server.Api.Controllers;

[Authorize]
[Route("api/v1/[controller]")]
public class UsersController(IUserService userService) : Controller
{
    [HttpPatch("profile")]
    public async Task<IActionResult> UploadProfile([FromForm] IFormFile fileData)
    {
        try
        {
            return Ok(await userService.Upload(fileData, User.GetUserId()));
        }
        catch (InvalidDetailsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
    {
        try
        {
            return Ok(await userService.Update(request, User.GetUserId()));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}