using Cloud.DTOs;
using Cloud.DTOs.File;

namespace Cloud.Interfaces.Services;

public interface IFileServices
{
    public Task<string> UploadFileAsync(int userId, IFormFile file);

    public Task<bool> DeleteAnyFileAsync(int fileId);
    
    public Task<bool> DeleteUserFileAsync(int userId, int fileId);

    public Task<FileDTOs> GetFileByIdAsync(int fileId);

    public Task<FileDTOs> GetMyFileAsync(int fileId, int fileOwnerId);

    public Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId);

    public Task<IEnumerable<FileDTOs>> GetAllFilesAsync();

    public Task<(Stream?, string fileName)?> DownloadFileAsync(int fileId);
    
    public Task<(Stream?, string fileName)?> DownloadMyFileAsync(int userId, int fileId);
    
}