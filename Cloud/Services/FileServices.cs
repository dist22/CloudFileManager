using AutoMapper;
using Cloud.Interfaces;
using Cloud.Models;
using Cloud.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Services;

public class FileServices : IFileServices
{
    private readonly IUserRepository _userRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IFileSizeConverter _fileSizeConverter;
    private readonly IMapper _mapper;

    public FileServices(IUserRepository userRepository, IFileRepository fileRepository, IMapper mapper,
        IBlobStorage blobStorage, IFileSizeConverter fileSizeConverter)
    {
        _userRepository = userRepository;
        _fileRepository = fileRepository;
        _mapper = mapper;
        _blobStorage = blobStorage;
        _fileSizeConverter = fileSizeConverter;
    }


    public async Task<string> UploadFileAsync(int userId, IFormFile file)
    {
        var user = await _userRepository.GetUser(u => u.userId == userId);

        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{originalFileName}_{Guid.NewGuid()}{extension}";

        var fileUrl = await _blobStorage.UploadAsync(uniqueFileName, file, user);

        var fileRecord = new FileRecord
        {
            fileName = uniqueFileName,
            filePath = fileUrl,
            fileSize = _fileSizeConverter.FormatSize(file.Length),
            fileType = file.ContentType,
            userId = user.userId
        };

        await _fileRepository.AddFileAsync(fileRecord);
        user.files.Add(fileRecord);
        await _userRepository.EditUser(user);

        return fileUrl;
    }

    public async Task<bool> DeleteAnyFileAsync(int fileId)
    {
        var file = await _fileRepository.GetFileAsync(fileId);
        var user = await _userRepository.GetUser(u => u.userId == file.userId);

        var resultAzure = await _blobStorage.DeleteAsync(file, user);

        var resultFile = await _fileRepository.DeleteFileAsync(file);

        return resultFile && resultAzure;
    }

    public async Task<bool> DeleteUserFileAsync(int userId, int fileId)
    {
       var file = await _fileRepository.GetFileAsync(fileId);
       if (file == null)
           return false;
       
       var user = await _userRepository.GetUser(u => u.userId == userId);
       if (user == null)
           return false;
       
       if (file.userId != userId)
           return false;
       
       return await _fileRepository.DeleteFileAsync(file) 
              && await _blobStorage.DeleteAsync(file, user);
    }

    public async Task<FileDTOs> GetFileByIdAsync(int fileId)
    {
        var file = await _fileRepository.GetFileAsync(fileId);
        return _mapper.Map<FileDTOs>(file);
    }

    public async Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId)
    {
        var files = await _fileRepository.GetFilesAsync();
        var result = files.Where(f => f.userId == userId).ToList();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<IEnumerable<FileDTOs>> GetAllFilesAsync()
    {
        var result = await _fileRepository.GetFilesAsync();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<(Stream?, string fileName)?> DownloadFileAsync(int fileId)
    {
        var file = await _fileRepository.GetFileAsync(fileId);
        var user = await _userRepository.GetUser(u => u.userId == file.userId);
        var stream = await _blobStorage.DownloadAsync(file, user);
        return (stream, file.fileName);
    }
}