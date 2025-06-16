namespace Cloud.Interfaces;

public interface IBlobStorage
{
    public Task<string> UploadAsync(string uniqueFileName, IFormFile file);
    public Task<bool> DeleteAsync(string fileName);
    public Task<Stream?> DownloadAsync(string fileName);
}