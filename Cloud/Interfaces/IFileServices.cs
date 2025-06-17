using Cloud.DTOs;
using Cloud.Models;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Interfaces;

public interface IFileServices
{
    public Task<string> UploadFileAsync(int userId, IFormFile file);

    public Task<bool> DeleteFileAsync(int fileId);

    public Task<FileDTOs> GetFileByIdAsync(int fileId);

    public Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId);

    public Task<IEnumerable<FileDTOs>> GetAllFilesAsync();

    public Task<(Stream?, string fileName)?> DownloadFileAsync(int fileId);

}