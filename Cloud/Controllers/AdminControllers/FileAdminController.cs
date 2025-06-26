using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs.File;
using Cloud.Interfaces.Services;

namespace Cloud.Controllers.AdminControllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/files")]

public class FileAdminController(IFileServices fileServices) : ControllerBase
{
    
    [HttpGet]
    public async Task<IEnumerable<FileDTOs>> GetAllFilesController() 
        => await fileServices.GetAllFilesAsync();
    
    [HttpGet("file/{fileId}")]
    public async Task<FileDTOs> GetFileController(int fileId) 
        => await fileServices.GetFileByIdAsync(fileId);
    
    [HttpGet("user-files/{userId}")]
    public async Task<IEnumerable<FileDTOs>> GetUserFilesController(int userId)
        => await fileServices.GetAllUserFilesAsync(userId);

    
    [HttpGet("{fileId}/download")]
    public async Task<IActionResult> DownloadFileController(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadFileAsync(fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }
    
    [HttpDelete("{fileId}")]
    public async Task<IActionResult> DeleteFileController(int fileId)
        => await fileServices.DeleteAnyFileAsync(fileId) ? Ok("Success") : NotFound();
    
}