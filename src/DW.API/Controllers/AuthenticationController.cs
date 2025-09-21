using DW.API.Models.Auth;
using DW.API.Validations;
using DW.Application.Common;
using DW.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DW.API.Controllers;

[Route("api/auth")]
public class AuthenticationController : BaseApiController
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
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var validator = new RegisterRequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var validationErrors = validationResult
                .Errors.Select(error => Error.ValidationFailed(error.ErrorMessage)).ToList();

            var failureResult = Result.Failure(validationErrors);
            return HandleResult(failureResult);    
        }

        var registerResult = await _userManagementService.RegisterUserAsync(request.Email, request.Password);

        if (!registerResult.IsSuccess)
        {
            return HandleResultWithStatus(registerResult);
        }
        
        var accessToken = _tokenService.GenerateAccessToken(registerResult.Data.Id, registerResult.Data.Email);
        var refreshToken = await _refreshTokenService.GenerateRefreshTokenAsync(registerResult.Data.Id);

        var response = new RegisterResponse
        {
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