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
        => await userServices.GetUsersAsync();

    [Authorize(Roles = "Admin")]
    [HttpGet("GetUserById/{userId}")]
    public async Task<IActionResult> GetUserController(int userId)
        => Ok(await userServices.GetUserAsync(userId));

    [Authorize(Roles = "Admin, User")]
    [HttpGet("GetMyUser")]
    public async Task<IActionResult> GetMyUserController()
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaimId == null)
            return Problem();
        var userId = System.Convert.ToInt32(userClaimId);
        return Ok(await userServices.GetUserAsync(userId));
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("EditUserRole")]
    public async Task<IActionResult> EditUserRoleController([FromForm] UserEditDTO userEditDto)
        => await userServices.EditUserAsync(userEditDto) ? Ok("Success") : Problem();

    [Authorize(Roles = "Admin")]
    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUserController(int userId) 
        => await userServices.DeleteUserAsync(userId) ? Ok("Success") : Problem();
}