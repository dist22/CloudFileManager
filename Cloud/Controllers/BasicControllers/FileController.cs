using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Authorize(Roles = "Admin, User")]
[Route("api/user/files")]

public class FileController(IFileServices fileServices) : ControllerBase
{
    [HttpGet("GetMyFile/{fileId}")]
    public async Task<IActionResult> GetMyFileController(int fileId)
        => Ok(await fileServices.GetMyFileAsync(fileId, this.GetUserId()));
    
    [HttpGet("GetMyFiles")]
    public async Task<IEnumerable<FileDTOs>> GetMyFilesController() 
        => await fileServices.GetAllUserFilesAsync(this.GetUserId());
    
    [HttpGet("DownloadMyFile/{fileId}")]
    public async Task<IActionResult> DownloadMyFile(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadMyFileAsync(this.GetUserId(),fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }
    
    [HttpPost("Upload")]
    public async Task<string> UploadFileController(IFormFile file) 
        => await fileServices.UploadFileAsync(this.GetUserId(), file);
    
    
    [HttpDelete("DeleteMyFile/{fileId}")]
    public async Task<IActionResult> DeleteMyFileController(int fileId) 
        => await fileServices.DeleteUserFileAsync(this.GetUserId(), fileId) ?
            Ok("Success") : NotFound();
}