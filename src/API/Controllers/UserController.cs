using API.Interfaces;
using API.Models.DbModels;
using Common.OperatingModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("/[controller]/[action]")]
public class UserController : Controller
{
    private IUserService _userService;

    public UserController(IUserService userService)
    {
        this._userService = userService;
    }
    
    [Authorize]
    [HttpGet("/getuser")]
    public async Task<IActionResult> GetUserAsync(string email)
    {
        var userResult = await _userService.GetUserByEmailAsync(email);
        if (!userResult.IsSuccess)
        {
            if (userResult.Status == GetEntityResult<User>.ResultType.NotFound)
            {
                return BadRequest(new { errorText = "User not found." });
            }
            if (userResult.Status == GetEntityResult<User>.ResultType.DatabaseError)
            {
                return BadRequest(new { errorText = "Database error." });
            }
        }

        var user = userResult.Entity;
        return new JsonResult(user);
    }
}