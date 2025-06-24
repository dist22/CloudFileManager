using AutoMapper;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Models;
using Cloud.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class FileRepository(DataContextEF entity) : BaseRepository<FileRecord>(entity), IFileRepository
{
    public async Task AddFileAsync(FileRecord file)
    {
        await _dbSet.AddAsync(file);
        if (!await SaveChangesAsync())
            throw new Exception("Error");
    }

    public async Task<IEnumerable<FileRecord>> GetFilesAsync() 
        => await _dbSet
            .AsNoTracking()
            .ToListAsync();

    public async Task<FileRecord?> GetFileAsync(int id) 
        => await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.fileId == id);

    public async Task<FileRecord> GetFileIfExistAsync(int fileId)
    {
        var file = await GetFileAsync(fileId);
        if(file == null)
            throw new FileNotFoundException("File not found");
        return file;
    }

    public async Task<bool> DeleteFileAsync(FileRecord file) 
        => await Remove(file) ? true : throw new Exception("Error");
}