using Cloud.Application.DTOs.User;
using Cloud.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers.AdminControllers;


[ApiController]
[Route("api/admin/users")]
[Authorize(Roles = "Admin")]
public class UserAdminController(IUserServices userServices) : ControllerBase
{
    [HttpGet]
    public async Task<IEnumerable<UserDTOs>> GetUsersController() 
        => await userServices.GetUsersAsync();
    
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserController(int userId)
        => Ok(await userServices.GetUserAsync(userId));

    [HttpPut("{userId}/{role}")]
    public async Task<IActionResult> EditUserRoleController(int userId, string role)
    {
        await userServices.EditUserAsync(userId, role);
        return Ok("User edited");
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserController(int userId)
    {
        await userServices.DeleteUserAsync(userId);
        return Ok("User deleted");
    }

}