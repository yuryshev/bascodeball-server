using System.Security.Claims;
using API.Services;
using API.Stores;
using Common.OperatingModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/[controller]/[action]")]
public class AuthorizationController : Controller
{
    private GetIdentityService _service;
    private UserStore _store;

    public AuthorizationController(GetIdentityService service, UserStore store)
    {
        _service = service;
        _store = store;
    }
    
    [HttpPost("/token")]
    public async Task<IActionResult> Token(string email)
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
    public async Task<IActionResult> Registration(string email, string loginName)
    {
        var userResult = await this._store.AddUserAsync(email, loginName);
        if (!userResult.IsSuccess)
        {
            return BadRequest(new { errorText = "Database error." });
        }

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
}