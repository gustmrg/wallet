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
            message = result.FirstError ?? "An error occurred"
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
            message = result.FirstError ?? "An error occurred"
        });
    }
    
    protected IActionResult HandleResultWithStatus<T>(Result<T> result, 
        int successStatusCode = 200)
    {
        if (result.IsSuccess)
        {
            return StatusCode(successStatusCode, result.Data);
        }

        // Analyze errors to determine appropriate status code
        var statusCode = DetermineErrorStatusCode(result);
            
        return StatusCode(statusCode, new
        {
            errors = result.Errors,
            message = result.FirstError ?? "An error occurred"
        });
    }
    
    private int DetermineErrorStatusCode<T>(Result<T> result)
    {
        if (result.Errors.Any(e => e.Contains("not found", StringComparison.OrdinalIgnoreCase)))
        {
            return 404;
        }
            
        if (result.Errors.Any(e => e.Contains("unauthorized", StringComparison.OrdinalIgnoreCase)))
        {
            return 401;
        }
            
        if (result.Errors.Any(e => e.Contains("forbidden", StringComparison.OrdinalIgnoreCase)))
        {
            return 403;
        }
            
        if (result.Errors.Any(e => e.Contains("validation", StringComparison.OrdinalIgnoreCase)))
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
            message = result.FirstError ?? "Creation failed"
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
            message = result.FirstError ?? "Deletion failed"
        });
    }
}