using System.Security.Claims;
using API.Interfaces;
using API.Services;
using API.Stores;
using Common.OperatingModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/[controller]/[action]")]
public class AuthorizationController : Controller
{
    private IIdentityService _service;

    public AuthorizationController(IIdentityService service)
    {
        _service = service;
    }
    
    [HttpPost("/token")]
    public async Task<IActionResult> TokenAsync(string email)
    {
        var identityResult = await _service.GetIdentity(email);
        if (!identityResult.IsSuccess)
        {
            if (identityResult.Status == GetEntityResult<ClaimsIdentity>.ResultType.NotFound)
            {
                return BadRequest(new { errorText = "Invalid email." });
            }
            if (identityResult.Status == GetEntityResult<ClaimsIdentity>.ResultType.UnexpectedError)
            {
                return BadRequest(new { errorText = "Unexpected error." });
            }
            if (identityResult.Status == GetEntityResult<ClaimsIdentity>.ResultType.DatabaseError)
            {
                return BadRequest(new { errorText = "Database error." });
            }
        }

        var identity = identityResult.Entity;
        return GenerateJwtService.GenerateJwtToken(identity);
    }
    
    [HttpPost("/reg")]
    public async Task<IActionResult> RegistrationAsync(string email, string loginName)
    {
        var identityResult = await _service.RegIdentity(email, loginName);
        if (!identityResult.IsSuccess)
        {
            if (identityResult.Status == GetEntityResult<ClaimsIdentity>.ResultType.UnexpectedError)
            {
                return BadRequest(new { errorText = "Unexpected error." });
            }
            if (identityResult.Status == GetEntityResult<ClaimsIdentity>.ResultType.DatabaseError)
            {
                return BadRequest(new { errorText = "Database error." });
            }
        }

        var identity = identityResult.Entity;
        return GenerateJwtService.GenerateJwtToken(identity);
    }
}