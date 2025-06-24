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
    {
        return await _dbSet
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<FileRecord?> GetFileAsync(int id)
    {
        return await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.fileId == id);
    }

    public async Task<bool> DeleteFileAsync(FileRecord file)
    {
        return await Remove(file) ? true : throw new Exception("Error");
    }
}