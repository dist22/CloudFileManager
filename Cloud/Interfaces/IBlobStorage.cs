using Cloud.Models;

namespace Cloud.Interfaces;
public interface IBlobStorage
{
    public Task<string> CreateUserContainerAsync(string userId);
    public Task<bool> DeleteUserContainerAsync(User user);
    public Task<string> UploadAsync(string uniqueFileName, IFormFile file, User user);
    public Task<bool> DeleteAsync(FileRecord file, User? user);
    public Task<Stream?> DownloadAsync(FileRecord file, User user);
    
}