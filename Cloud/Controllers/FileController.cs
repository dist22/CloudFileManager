using System.Security.Claims;
using Cloud.DTOs;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]

public class FileController(IFileServices fileServices) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpGet("GetAllFiles")]
    public async Task<IEnumerable<FileDTOs>> GetAllFilesController() 
        => await fileServices.GetAllFilesAsync();

    [Authorize(Roles = "Admin")]
    [HttpGet("GetFile/{fileId}")]
    public async Task<FileDTOs> GetFileController(int fileId) 
        => await fileServices.GetFileByIdAsync(fileId);


    [Authorize(Roles = "Admin")]
    [HttpGet("GetUserFiles/{userId}")]
    public async Task<IEnumerable<FileDTOs>> GetUserFilesController(int userId) 
        => await fileServices.GetAllUserFilesAsync(userId);

    [Authorize(Roles = "Admin,User")]
    [HttpGet("GetMyFiles")]
    public async Task<IEnumerable<FileDTOs>> GetMyFilesController()
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaimId == null)
            throw new UnauthorizedAccessException();
        var userId = System.Convert.ToInt32(userClaimId);
        return await fileServices.GetAllUserFilesAsync(userId);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("Download/{fileId}")]
    public async Task<IActionResult> DownloadFileController(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadFileAsync(fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost("Upload")]
    public async Task<string> UploadFileController(IFormFile file)
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaimId == null)
            throw new UnauthorizedAccessException();
        var userId = System.Convert.ToInt32(userClaimId);
        return await fileServices.UploadFileAsync(userId, file);
    }


    [Authorize(Roles = "Admin,User")]
    [HttpDelete("Delete/{fileId}")]
    public async Task<IActionResult> DeleteFileController(int fileId)
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaimId == null)
            throw new UnauthorizedAccessException();
        var userId = System.Convert.ToInt32(userClaimId);
        return await fileServices.DeleteFileAsync(userId, fileId) ? Ok("Success") : Forbid();
    }

}