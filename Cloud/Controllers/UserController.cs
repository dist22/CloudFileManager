using AutoMapper;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs;
using Cloud.Models;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UserController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    #region MyRegion


    [HttpGet("GetListOfUsers")]
    public async Task<IEnumerable<User>> GetListOfUsersController()
    {
        return await _userRepository.GetListOfUsers();
    }

    [HttpGet("GetUserById/{id}")]
    public async Task<UserDTOs> GetUserByIdController(int id)
    {
        var user = await _userRepository.GetUserById(id);
        return _mapper.Map<UserDTOs>(user);
    }

    
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUserController(UserForCreate userForCreate)
    {
        if (!await _userRepository.GetUserByEmail(userForCreate.email) &&
            !await _userRepository.GetUserByUserName(userForCreate.username))
        {
            await _userRepository.CreateUser(userForCreate);
            return Ok("Successful");
        }

        return Conflict("User with this email or username already registered");
    }
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]

    #endregion
}