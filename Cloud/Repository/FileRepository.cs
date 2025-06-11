using AutoMapper;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.Models;
using Cloud.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class FileRepository : IFileRepository
{
    private readonly DataContextEF _entity;
    private readonly IMapper _mapper;
    private readonly IBlobStorage _blobStorage;

    public FileRepository(DataContextEF entity, IBlobStorage blobStorage, IMapper mapper)
    {
        _entity = entity;
        _blobStorage = blobStorage;
        _mapper = mapper;
    }

    public async Task<IEnumerable<FileDTOs>> GetUserFiles(int id)
    {
        var files = await _entity.Files
            .AsNoTracking()
            .Where(f => f.userId == id)
            .ToListAsync() ?? throw new Exception("Error");
        
        var result = _mapper.Map<IEnumerable<FileDTOs>>(files);
        return result;
    }

    public async Task<IEnumerable<FileDTOs>> GetListOfFiles()
    {
        var fileList = await _entity.Files
            .AsNoTracking()
            .ToListAsync();

        var result = _mapper.Map<IEnumerable<FileDTOs>>(fileList);
        return result;
    }


    public async Task<string> UploadFile(IFormFile userFile)
    {
        var fileURL = await _blobStorage.UploadAsync(userFile);

        var file = new FileRecord
        {
            fileName = userFile.FileName,
            filePath = fileURL,
            fileSize = userFile.Length,
            fileType = userFile.ContentType,
            uploadedAt = DateTime.Now,
            userId = 23
        };

        await _entity.Files.AddAsync(file);
        if (!(await _entity.SaveChangesAsync() > 0))
            throw new Exception("Error");

        return fileURL;

    }


}