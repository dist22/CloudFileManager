using Cloud.Models;

namespace Cloud.Interfaces;

public interface IFileRepository
{
    public Task AddFileAsync(FileRecord file);

    public Task<IEnumerable<FileRecord>> GetAllFilesAsync();

    public Task<FileRecord?> GetFileByIdAsync(int id);

    protected Task<bool> SaveChangesASync();

    public Task<bool> DeleteFileAsync(FileRecord file);
}