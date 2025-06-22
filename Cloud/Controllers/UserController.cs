using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(IUserServices userServices) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("GetUsers")]
    public async Task<IEnumerable<UserDTOs>> GetUsersController()
    {
        return await userServices.GetUsersAsync();
    }

    [HttpGet("GetUserById/{userId}")]
    public async Task<UserDTOs> GetUserController(int userId)
    {
        return await userServices.GetUserAsync(userId);
    }

    [HttpPut("EditUser/{userId}")]
    public async Task<IActionResult> EditUserController(int userId, [FromForm]UserEditDTO userEditDto)
    {
        var result = await userServices.EditUserAsync(userId, userEditDto);
        return result ? Ok("Successful") : Problem();
    }

    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUserController(int userId)
    {
        var result = await userServices.DeleteUserAsync(userId);
        return result ? Ok("Successful") : Conflict();
    }
}