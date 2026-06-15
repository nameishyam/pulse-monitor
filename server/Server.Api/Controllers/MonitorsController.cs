using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Server.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/[controller]")]
public class MonitorsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post()
    {
        
    }
}