using Cloud.Application.DTOs.User;
using Cloud.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers.AuthController;

[ApiController]
[AllowAnonymous]
[Route("api/auth")]
public class AuthController(IAuthServices authServices) : ControllerBase
{
    [HttpPost("reg")]
    public async Task<IActionResult> RegUserController([FromForm] UserCreateDTO userCreateDto)
        => await authServices.RegisteredAsync(userCreateDto) ? Ok("Successful") : Conflict();

    [HttpPost("log")]
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