using Cloud.Models;

namespace Cloud.Interfaces;

public interface IFileRepository
{
    public Task AddFileAsync(FileRecord file);

    public Task<IEnumerable<FileRecord>> GetFilesAsync();

    protected internal Task<FileRecord?> GetFileAsync(int id);

    public Task<FileRecord> GetFileIfExistAsync(int id);
    
    public Task<bool> DeleteFileAsync(FileRecord file);
}