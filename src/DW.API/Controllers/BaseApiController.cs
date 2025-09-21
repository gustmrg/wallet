using DW.Application.Common;
using Microsoft.AspNetCore.Mvc;

namespace DW.API.Controllers;

[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected IActionResult HandleResult(Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }
        
        return BadRequest(new
        {
            errors = result.Errors,
        });
    }
    
    protected IActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Data);
        }
        
        return BadRequest(new
        {
            errors = result.Errors,
        });
    }
    
    protected IActionResult HandleResultWithStatus<T>(Result<T> result, 
        int successStatusCode = 200)
    {
        if (result.IsSuccess)
        {
            return StatusCode(successStatusCode, result.Data);
        }

        var statusCode = DetermineErrorStatusCode(result);
            
        return StatusCode(statusCode, new
        {
            errors = result.Errors,
        });
    }
    
    private int DetermineErrorStatusCode<T>(Result<T> result)
    {
        if (result.Errors.Any(e => e.Code.Contains("NOT_FOUND", StringComparison.OrdinalIgnoreCase)))
        {
            return 404;
        }
            
        if (result.Errors.Any(e => e.Code.Contains("UNAUTHORIZED", StringComparison.OrdinalIgnoreCase)))
        {
            return 401;
        }
            
        if (result.Errors.Any(e => e.Code.Contains("FORBIDDEN", StringComparison.OrdinalIgnoreCase)))
        {
            return 403;
        }
        
        if (result.Errors.Any(e => e.Code.Contains("CONFLICT", StringComparison.OrdinalIgnoreCase)))
        {
            return 409;
        }
            
        if (result.Errors.Any(e => e.Code.Contains("VALIDATION", StringComparison.OrdinalIgnoreCase)))
        {
            return 422;
        }
        
        return 400;
    }
    
    protected IActionResult HandleCreationResult<T>(Result<T> result, string actionName, object routeValues = null)
    {
        if (result.IsSuccess)
        {
            return CreatedAtAction(actionName, routeValues, result.Data);
        }

        return BadRequest(new
        {
            errors = result.Errors,
        });
    }
    
    protected IActionResult HandleDeletionResult(Result result)
    {
        if (result.IsSuccess)
        {
            return NoContent(); // 204 - standard for successful deletion
        }

        return BadRequest(new
        {
            errors = result.Errors,
        });
    }
}