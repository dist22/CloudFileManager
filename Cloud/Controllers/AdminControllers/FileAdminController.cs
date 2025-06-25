using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cloud.DTOs;
using Cloud.Interfaces;
namespace Cloud.Controllers.AdminControllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin/file")]

public class FileAdminController(IFileServices fileServices) : ControllerBase
{
    
    [HttpGet("GetAllFiles")]
    public async Task<IEnumerable<FileDTOs>> GetAllFilesController() 
        => await fileServices.GetAllFilesAsync();
    
    [HttpGet("GetFile/{fileId}")]
    public async Task<FileDTOs> GetFileController(int fileId) 
        => await fileServices.GetFileByIdAsync(fileId);
    
    [HttpGet("GetUserFiles/{userId}")]
    public async Task<IEnumerable<FileDTOs>> GetUserFilesController(int userId)
        => await fileServices.GetAllUserFilesAsync(userId);

    
    [HttpGet("Download/{fileId}")]
    public async Task<IActionResult> DownloadFileController(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadFileAsync(fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }
    
    [HttpDelete("Delete/{fileId}")]
    public async Task<IActionResult> DeleteFileController(int fileId)
        => await fileServices.DeleteAnyFileAsync(fileId) ? Ok("Success") : NotFound();
    
}