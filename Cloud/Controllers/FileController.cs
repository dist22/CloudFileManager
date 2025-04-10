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

    #region MyRegion

    // [HttpGet]
    // [HttpPost]
    // [HttpPut]
    // [HttpDelete]

    #endregion
    
}