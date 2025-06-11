using Cloud.Models;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;


    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet("GetListOfUsers")]
    public async Task<IEnumerable<UserDTOs>> GetListOfUsersController()
    {
        return await _userRepository.GetListOfUsers();
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<UserDTOs> GetUserByIdController(int id)
    {
        return await _userRepository.GetUserById<UserDTOs>(id);
    }


    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUserController(UserForCreate userForCreate)
    {
        if (!await _userRepository.UserExists(u => u.email == userForCreate.email) &&
            !await _userRepository.UserExists(u => u.username == userForCreate.username))
        {
            await _userRepository.CreateUser(userForCreate);
            return Ok("Successful");
        }

        return Conflict("User with this email or username already registered");
    }

    [HttpPut("EditUser/{id}")]
    public async Task<IActionResult> EditUserController(int id, UserForEdit userForEdit)
    {
        var user = await _userRepository.GetUserById<User>(id);
        await _userRepository.EditUser(user, userForEdit);
        return Ok();
    }

    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUserController(int id)
    {
        var user = await _userRepository.GetUserById<User>(id);
        await _userRepository.DeleteUser(user);
        return Ok();
    }
}