using Cloud.DTOs;
using Cloud.Models;

namespace Cloud.Interfaces;

public interface IFileRepository
{
    public Task<string> UploadFile(IFormFile userFile);

    public Task<IEnumerable<FileDTOs>> GetListOfFiles();
    
    public Task<IEnumerable<FileDTOs>> GetUserFiles(int id);
}