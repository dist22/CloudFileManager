using Cloud.DTOs;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    [HttpPost("Reg")]
    public async Task<IActionResult> RegUserController([FromForm] UserCreateDTO userCreateDto)
        => await authServices.RegisteredAsync(userCreateDto) ? Ok("Successful") : Conflict();

    [HttpPost("Login")]
    public async Task<IActionResult> LoginController([FromForm] UserLoginDTO userLoginDto)
    {
        var token = await authServices.LoginAsync(userLoginDto);
        
        Response.Cookies.Append("token", token);
        
        return Ok(new Dictionary<string, string>
        {
            { "token:", token }
        });
    }
}