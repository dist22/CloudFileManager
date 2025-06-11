namespace Cloud.Interfaces;

public interface IBlobStorage
{
    public Task<string> UploadAsync(IFormFile file);
    public Task<bool> DeleteAsync(string fileName);
}