using API.Interfaces;
using Common.DbModels;
using Common.DTOModels;
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
    [HttpGet("/getUser")]
    public async Task<IActionResult> GetUserAsync()
    {
        var userResult = await _userService.GetUserByEmailAsync(User.Identity.Name);
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
    
    [HttpPost("/updateUserRating")]
    public async Task<IActionResult> UpdateUsersRating([FromBody] List<UserDTO> users)
    {
        foreach (var inputUserDTO in users)
        {
            var userResult = await _userService.UpdateUserRating(inputUserDTO.Email, inputUserDTO.Rating);
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
        }
        
        return new OkResult();
    }
    
    [HttpGet("/getUserRating")]
    public async Task<IActionResult> GetUsersRating()
    {
        var userListResult = await _userService.GetUsersRating();
        if (!userListResult.IsSuccess)
        {
            if (userListResult.Status == GetEntityResult<List<User>>.ResultType.NotFound)
            {
                return BadRequest(new { errorText = "Users not found." });
            }
            if (userListResult.Status == GetEntityResult<List<User>>.ResultType.DatabaseError)
            {
                return BadRequest(new { errorText = "Database error." });
            }
        }

        var userList = userListResult.Entity;
        return new JsonResult(userList);
    }
}