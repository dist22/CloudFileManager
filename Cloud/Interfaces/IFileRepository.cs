using Cloud.Models;

namespace Cloud.Interfaces;

public interface IFileRepository
{
    public Task AddFileAsync(FileRecord file);

    public Task<IEnumerable<FileRecord>> GetFilesAsync();

    public Task<FileRecord?> GetFileAsync(int id);
    
    public Task<bool> DeleteFileAsync(FileRecord file);
}