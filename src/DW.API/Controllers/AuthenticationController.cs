using System.Linq;
using DW.API.Models.Auth;
using DW.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DW.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthenticationController : ControllerBase
{
    private readonly IUserManagementService _userManagementService;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthenticationController(
        IUserManagementService userManagementService, 
        ITokenService tokenService, 
        IRefreshTokenService refreshTokenService)
    {
        _userManagementService = userManagementService;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
    }
    
    [HttpPost("register")]
    public async Task<ActionResult<RegisterResponse>> Register(RegisterRequest request)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToArray();

            return BadRequest(new RegisterResponse
            {
                IsSuccess = false,
                Message = string.Join("; ", errors)
            });
        }

        var registerResult = await _userManagementService.RegisterUserAsync(request.Email, request.Password);

        if (!registerResult.IsSuccess)
        {
            return BadRequest();
        }
        
        var accessToken = _tokenService.GenerateAccessToken(registerResult.Data.Id, registerResult.Data.Email);
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(registerResult.Data.Id);

        var response = new RegisterResponse
        {
            IsSuccess = true,
            Message = "User registered successfully",
            UserId = registerResult.Data.Id,
            Email = registerResult.Data.Email,
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };

        return Ok(response);
    }
    
    [HttpPost("login")]
    public IActionResult Login(LoginRequest loginRequest)
    {
        return Ok();
    }
}