﻿using System.Security.Claims;
using Cloud.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Cloud.Extensions;
using Microsoft.AspNetCore.Authorization;

namespace Cloud.Controllers.BasicControllers;

[ApiController]
[Route("api/user/users")]
[Authorize(Roles = "Admin,User")]
public class UserController(IUserServices userServices) : ControllerBase
{

    [HttpGet("me")]
    public async Task<IActionResult> GetMyUserController()
        => Ok(await userServices.GetUserAsync(this.GetUserId()));

    [HttpDelete("me")]
    public async Task<IActionResult> DeleteMyUser()
    {
        await userServices.DeleteUserAsync(this.GetUserId());
        return Ok("User deleted");
    }

}