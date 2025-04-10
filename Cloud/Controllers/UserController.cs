using Cloud.Data;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

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

    #region MyRegion

    // [HttpGet]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]

    #endregion
}