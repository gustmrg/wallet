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
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(
        IUserManagementService userManagementService, 
        ITokenService tokenService, 
        IRefreshTokenService refreshTokenService, 
        IAuthenticationService authenticationService)
    {
        _userManagementService = userManagementService;
        _tokenService = tokenService;
        _refreshTokenService = refreshTokenService;
        _authenticationService = authenticationService;
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
    public async Task<IActionResult> Login(LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var authenticateResult = await _authenticationService.AuthenticateUserAsync(loginRequest.Email, loginRequest.Password);
        
        if (!authenticateResult.IsSuccess)
            return  HandleResultWithStatus(authenticateResult);
        
        var userResult = await _userManagementService.GetUserAsync(loginRequest.Email);

        if (!userResult.IsSuccess)
            return HandleResultWithStatus(userResult);

        var response = new LoginResponse
        {
            UserId = userResult.Data.Id,
            Email = userResult.Data.Email,
            AccessToken = authenticateResult.Data.AccessToken,
            RefreshToken = authenticateResult.Data.RefreshToken,
        };
        
        return Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshToken(RefreshTokenRequest request)
    {
        if (request == null)
            return BadRequest("Request cannot be null");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        // Get user ID from the refresh token
        var userId = await _refreshTokenService.GetUserIdFromTokenAsync(request.RefreshToken);

        if (!userId.HasValue)
            return Unauthorized("Invalid or expired refresh token");

        // Validate the refresh token
        var validationResult = await _refreshTokenService.ValidateRefreshTokenAsync(request.RefreshToken);

        if (!validationResult.IsSuccess)
            return Unauthorized("Invalid or expired refresh token");

        // Generate new tokens
        var tokenResult = await _authenticationService.IssueNewTokensAsync(userId.Value);

        if (!tokenResult.IsSuccess)
            return HandleResultWithStatus(tokenResult);

        // Revoke the used refresh token
        await _refreshTokenService.RevokeRefreshTokenAsync(request.RefreshToken, "Token refreshed");

        var response = new LoginResponse
        {
            UserId = userId.Value,
            Email = "", // We don't expose email in refresh response for security
            AccessToken = tokenResult.Data.AccessToken,
            RefreshToken = tokenResult.Data.RefreshToken,
        };

        return Ok(response);
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        return Ok();
    }
}