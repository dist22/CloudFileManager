using System.Security.Claims;
using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Extensions;
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
        => await fileServices.GetAllUserFilesAsync(this.GetUserId());

    [Authorize(Roles = "Admin")]
    [HttpGet("Download/{fileId}")]
    public async Task<IActionResult> DownloadFileController(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadFileAsync(fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet("DownloadMyFile/{fileId}")]
    public async Task<IActionResult> DownloadMyFile(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadMyFileAsync(this.GetUserId(),fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }

    [Authorize(Roles = "Admin,User")]
    [HttpPost("Upload")]
    public async Task<string> UploadFileController(IFormFile file) 
        => await fileServices.UploadFileAsync(this.GetUserId(), file);
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("Delete/{fileId}")]
    public async Task<IActionResult> DeleteFileController(int fileId)
        => await fileServices.DeleteAnyFileAsync(fileId) ? Ok("Success") : NotFound();

    [Authorize(Roles = "Admin, User")]
    [HttpDelete("DeleteMyFile/{fileid}")]
    public async Task<IActionResult> DeleteMyFileController(int fileId) 
        => await fileServices.DeleteUserFileAsync(this.GetUserId(), fileId) ?
            Ok("Success") : NotFound();

}