using System.Security.Claims;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController(IUserServices userServices) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("GetUsers")]
    public async Task<IEnumerable<UserDTOs>> GetUsersController()
    {
        return await userServices.GetUsersAsync();
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("GetUserById/{userId}")]
    public async Task<UserDTOs> GetUserController(int userId)
    {
        return await userServices.GetUserAsync(userId);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetMyUser")]
    public async Task<UserDTOs> GetMyUserController()
    {
        var userId = System.Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        return await userServices.GetUserAsync(userId);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("EditUserRole")]
    public async Task<IActionResult> EditUserRoleController([FromForm] UserEditDTO userEditDto)
    {
        var result = await userServices.EditUserAsync(userEditDto);
        return result ? Ok("Successful") : Problem();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUserController(int userId)
    {
        var result = await userServices.DeleteUserAsync(userId);
        return result ? Ok("Successful") : Problem();
    }
}