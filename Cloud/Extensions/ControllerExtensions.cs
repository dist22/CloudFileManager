using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Extensions;

public static class ControllerExtensions
{
    public static int GetUserId(this ControllerBase controller)
    {
        var userClaimId = controller.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaimId == null)
            throw new UnauthorizedAccessException();
        return int.Parse(userClaimId);
    }
}