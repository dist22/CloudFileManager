using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]

public class FileController(IFileServices fileServices) : ControllerBase
{
    [HttpGet("GetAllFiles")]
    public async Task<IEnumerable<FileDTOs>> GetAllFilesController()
    {
        return await fileServices.GetAllFilesAsync();
    }

    [HttpGet("GetFile/{fileId}")]
    public async Task<FileDTOs> GetFileController(int fileId)
    {
        return await fileServices.GetFileByIdAsync(fileId);
    }

    [HttpGet("GetUserFiles/{userId}")]
    public async Task<IEnumerable<FileDTOs>> GetUserFilesController(int userId)
    {
        return await fileServices.GetAllUserFilesAsync(userId);
    }

    [HttpGet("Download/{fileId}")]
    public async Task<IActionResult> DownloadFileController(int fileId)
    {
        var (stream, fileName) = (await fileServices.DownloadFileAsync(fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }

    [HttpPost("Upload/{id}")]
    public async Task<string> UploadFileController(int id,IFormFile file)
    {
        return await fileServices.UploadFileAsync(id, file);
    }


    [HttpDelete("Delete/{fileId}")]
    public async Task<IActionResult> DeleteFileController(int fileId)
    {
        var result = await fileServices.DeleteFileAsync(fileId);
        return result ? Ok("Success") : NotFound("File with this id not found");
    }

}