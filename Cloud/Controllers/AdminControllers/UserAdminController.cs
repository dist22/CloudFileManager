using Cloud.DTOs;
using Cloud.Interfaces;
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
    
    [HttpPut("EditUserRole")]
    public async Task<IActionResult> EditUserRoleController([FromForm] UserEditDTO userEditDto)
        => await userServices.EditUserAsync(userEditDto) ? Ok("Success") : Problem();
    
    [HttpDelete("{userId}")]
    public async Task<IActionResult> DeleteUserController(int userId) 
        => await userServices.DeleteUserAsync(userId) ? Ok("Success") : Problem();
    
}