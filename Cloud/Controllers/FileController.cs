using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]

public class FileController : ControllerBase
{
    private readonly IFileServices _fileServices;

    public FileController(IFileServices fileServices)
    {
        _fileServices = fileServices;
    }

    [HttpGet("GetAllFiles")]
    public async Task<IEnumerable<FileDTOs>> GetAllFilesController()
    {
        return await _fileServices.GetAllFilesAsync();
    }

    [HttpGet("GetUserFiles")]
    public async Task<IEnumerable<FileDTOs>> GetUserFilesController(int userId)
    {
        return await _fileServices.GetAllUserFilesAsync(userId);
    }

    [HttpGet("Download/{fileId}")]
    public async Task<IActionResult> DownloadFileController(int fileId)
    {
        var (stream, fileName) = (await _fileServices.DownloadFileAsync(fileId)).Value;
        return stream == null ? NotFound() : File(stream, "application/octet-stream", fileName);
    }

    [HttpPost("Upload/{id}")]
    public async Task<string> UploadFileController(int id,IFormFile file)
    {
        return await _fileServices.UploadFileAsync(id, file);
    }


    [HttpDelete("Delete/{fileId}")]
    public async Task<IActionResult> DeleteFileController(int fileId)
    {
        var result = await _fileServices.DeleteFileAsync(fileId);
        return result ? Ok("Success") : NotFound("File with this id not found");
    }

}