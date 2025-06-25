using Cloud.DTOs;
using Cloud.DTOs.File;
using Cloud.Interfaces;
using Cloud.Extensions;
using Cloud.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Authorize(Roles = "Admin, User")]
[Route("api/user/files")]

public class FileController(IFileServices fileServices) : ControllerBase
{
    [HttpGet("{fileId}")]
    public async Task<IActionResult> GetMyFileController(int fileId)
        => Ok(await fileServices.GetMyFileAsync(fileId, this.GetUserId()));
    
    [HttpGet]
    public async Task<IEnumerable<FileDTOs>> GetMyFilesController() 
        => await fileServices.GetAllUserFilesAsync(this.GetUserId());
    
    [HttpGet("{fileId}/download")]
    public async Task<IActionResult> DownloadMyFile(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadMyFileAsync(this.GetUserId(),fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }
    
    [HttpPost]
    public async Task<string> UploadFileController(IFormFile file) 
        => await fileServices.UploadFileAsync(this.GetUserId(), file);
    
    
    [HttpDelete("{fileId}")]
    public async Task<IActionResult> DeleteMyFileController(int fileId) 
        => await fileServices.DeleteUserFileAsync(this.GetUserId(), fileId) ?
            Ok("Success") : NotFound();
}