using Cloud.DTOs;
using Cloud.Models;

namespace Cloud.Interfaces;

public interface IFileServices
{
    public Task<string> UploadFileAsync(int userId, IFormFile file);

    public Task<bool> DeleteFileAsync(int userId, int fileId);

    public Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId);

    public Task<IEnumerable<FileDTOs>> GetAllFilesAsync();

}