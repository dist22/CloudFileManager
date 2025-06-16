using AutoMapper;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Models;
using Cloud.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class FileRepository : IFileRepository
{
    private readonly DataContextEF _entity;

    public FileRepository(DataContextEF entity)
    {
        _entity = entity;
    }

    public async Task AddFileAsync(FileRecord file)
    {
        await _entity.Files.AddAsync(file);
        if (!await SaveChangesASync())
            throw new Exception("Error");
    }

    public async Task<IEnumerable<FileRecord>> GetAllFilesAsync()
    {
        return await _entity.Files
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<FileRecord?> GetFileByIdAsync(int id)
    {
        return await _entity.Files
            .AsNoTracking()
            .FirstOrDefaultAsync(f => f.fileId == id) ?? 
               throw new Exception("File with this id not found");
    }

    public async Task<bool> SaveChangesASync()
    {
        return await _entity.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteFileAsync(FileRecord file)
    {
        _entity.Files.Remove(file);
        return await SaveChangesASync() ? true : throw new Exception("Error");
    }
}