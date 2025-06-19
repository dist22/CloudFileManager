using Cloud.DTOs;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{

    private readonly IAuthServices _authServices;

    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }

    [HttpPost("Reg")]
    public async Task<IActionResult> RegUserController([FromForm] UserCreateDTO userCreateDto)
        => await _authServices.RegisteredAsync(userCreateDto) ? Ok("Successful") : Conflict();

    [HttpPost("Login")]
    public async Task<IActionResult> LoginController([FromForm] UserLoginDTO userLoginDto)
        => await _authServices.LoginAsync(userLoginDto) ? Ok() : Conflict();

}