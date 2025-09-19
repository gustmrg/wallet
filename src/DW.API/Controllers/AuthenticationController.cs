using DW.Application.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace DW.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    [HttpPost("register")]
    public IActionResult Register(RegisterRequest request)
    {
        return Ok();
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        return Ok();
    }
}