using Cloud.Models;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserServices _userServices;
    public UserController(IUserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("GetUsers")]
    public async Task<IEnumerable<UserDTOs>> GetUsersController()
    {
        return await _userServices.GetUsersAsync();
    }

    [HttpGet("GetUserById/{userId}")]
    public async Task<UserDTOs> GetUserController(int userId)
    {
        return await _userServices.GetUserAsync(userId);
    }
    
    // [HttpPost("CreateUser")]
    // public async Task<IActionResult> CreateUserController([FromForm]UserCreateDTO userCreateDto)
    // {
    //     var result = await _userServices.CreateUserAsync(userCreateDto);
    //     return result ? Ok("Successful") : Problem();
    // }

    [HttpPut("EditUser/{userId}")]
    public async Task<IActionResult> EditUserController(int userId, [FromForm]UserEditDTO userEditDto)
    {
        var result = await _userServices.EditUserAsync(userId, userEditDto);
        return result ? Ok("Successful") : Problem();
    }

    [HttpDelete("DeleteUser/{userId}")]
    public async Task<IActionResult> DeleteUserController(int userId)
    {
        var result = await _userServices.DeleteUserAsync(userId);
        return result ? Ok("Successful") : Conflict();
    }
}