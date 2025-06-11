using Cloud.DTOs;
using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Controllers;

[ApiController]
[Route("[controller]")]

public class FileController : ControllerBase
{
    private readonly IFileRepository _fileRepository;

    public FileController(IFileRepository fileRepository)
    {
        _fileRepository = fileRepository;
    }

    [HttpGet("GetFiles")]
    public async Task<IEnumerable<FileDTOs>> GetFilesController()
    {
        return await _fileRepository.GetListOfFiles();
    }

    [HttpGet("GetUserFiles/{id}")]
    public async Task<IEnumerable<FileDTOs>> GetUserFilesController(int id)
    {
        return await _fileRepository.GetUserFiles(id);
    }

    [HttpPost("Upload")]
    public async Task<string> UploadController(IFormFile file)
    {
        return await _fileRepository.UploadFile(file);
    }
    
}