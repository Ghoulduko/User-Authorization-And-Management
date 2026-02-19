using AuthorizationIntegration.Application.Services;
using AuthorizationIntegration.Core.Entitites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationIntegration.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpGet("Login")]
    public IActionResult Login(string email, string password)
    {
        var token = _authService.Login(email, password);
        if (string.IsNullOrEmpty(token))
        {
            return BadRequest("Invalid email or password.");
        }

        return Ok(token);
    }

    [HttpGet("GetEmailVerificationCode")]
    public IActionResult GetEmailVerificationCode()
    {
        var email = User.FindFirst("Email")?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email claim not found in the token.");
        }

        var didTheCodeSend = _authService.SendVerificationCode(email);

        if (!didTheCodeSend)
        {
            return BadRequest("Failed to send the verification code. Please try again.");
        }

        return Ok($"Sent the verification code to the email: {email}");
    }

    [HttpGet("VerifyEmail")]
    public IActionResult VerifyEmailCode(string code)
    {
        var email = User.FindFirst("Email")?.Value;

        if (string.IsNullOrEmpty(email))
        {
            return BadRequest("Email claim not found in the token.");
        }

        var isCodeValid = _authService.VerifyEmail(email, code);
        if (!isCodeValid)
        {
            return BadRequest("Invalid verification code. Please try again.");
        }
        return Ok("Email verified successfully!");
    }


}
