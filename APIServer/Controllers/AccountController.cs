using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using ZLogger;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;

    public AccountController(ILogger<AccountController> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpGet]
    public IActionResult SigninGoogle()
    {
        _logger.LogInformation("GoogleLogin action was called");

        var properties = new AuthenticationProperties { RedirectUri = Url.Action(nameof(GoogleResponse)) };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("GoogleResponse")]
    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (result?.Succeeded != true)
        {
            _logger.LogWarning("GoogleResponse failure: result not succeeded");
            return BadRequest();
        }

        var emailClaim = result.Principal.Identities
                                .FirstOrDefault()?.Claims
                                .FirstOrDefault(x => x.Type == ClaimTypes.Email);

        if (emailClaim == null)
        {
            _logger.LogWarning("GoogleResponse failure: email claim not found");
            return BadRequest();
        }

        // You got user's email here.
        // You can register it or update it in your database
        string userEmail = emailClaim.Value;

        // TODO : other user data processing

        return Ok(result);
    }
}