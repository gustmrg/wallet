using DW.API.Models.Auth;
using DW.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DW.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;

    public AuthenticationController(IUserManagementService userManagementService)
    {
        _userManagementService = userManagementService;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _userManagementService.RegisterUserAsync(request.Email, request.Password);
        
        return Ok();
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        return Ok();
    }
}