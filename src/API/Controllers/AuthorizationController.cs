using System.Security.Claims;
using API.Services;
using Common.OperatingModels;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;
[ApiController]
[Route("/[controller]/[action]")]
public class AuthorizationController : Controller
{
    private GetIdentityService _service;

    public AuthorizationController(GetIdentityService service)
    {
        _service = service;
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
}