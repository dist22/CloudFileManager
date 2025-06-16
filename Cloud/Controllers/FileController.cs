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

    [HttpPost("Upload/{id}")]
    public async Task<string> UploadFileController(int id,IFormFile file)
    {
        return await _fileServices.UploadFileAsync(id, file);
    }


    [HttpDelete("Delete/{userid},{fileId}")]
    public async Task<IActionResult> DeleteFileController(int userId, int fileId)
    {
        var result = await _fileServices.DeleteFileAsync(userId, fileId);
        return result ? Ok() : NotFound();
    }

}