using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Api.Extensions;
using Server.Domain.Dto.Request.Auth;
using Server.Domain.Interfaces.Service;
using Server.Service.Exceptions;

namespace Server.Api.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    private const string CookieName = "access_token";

    [HttpPost("signup")]
    [AllowAnonymous]
    public async Task<IActionResult> Signup([FromBody] UserSignup request)
    {
        try
        {
            SetAuthCookie(await authService.SignupTask(request));

            return Ok("Signup successful");
        }
        catch (ConflictException e)
        {
            return Conflict(e.Message);
        }
        catch (InvalidDetailsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] UserLogin request)
    {
        try
        {
            SetAuthCookie(await authService.LoginTask(request));

            return Ok("Login successful");
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (InvalidDetailsException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("forgot")]
    [AllowAnonymous]
    public async Task<IActionResult> ForgotPassword([FromBody] string email)
    {
        try
        {
            await authService.GenerateOtp(email);

            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPost("verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyUser([FromBody] VerifyOtp request)
    {
        try
        {
            await authService.VerifyUser(request);

            return Ok();
        }
        catch (ForbidException e)
        {
            return BadRequest(e.Message);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch("reset")]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPassword request)
    {
        try
        {
            await authService.ResetUserPassword(request);

            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        try
        {
            return Ok(await authService.GetMeTask(User.GetUserId()));
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }


    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        try
        {
            Response.Cookies.Delete(CookieName);

            return Ok("Logout successful");
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    private void SetAuthCookie(string token)
    {
        Response.Cookies.Append(
            "access_token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
    }
}