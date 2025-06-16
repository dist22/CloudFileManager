using AutoMapper;
using Cloud.Interfaces;
using Cloud.Models;
using Cloud.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Cloud.Repository;

public class FileServices : IFileServices
{
    private readonly IUserRepository _userRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public FileServices(IUserRepository userRepository, IFileRepository fileRepository, IMapper mapper,
        IBlobStorage blobStorage)
    {
        _userRepository = userRepository;
        _fileRepository = fileRepository;
        _mapper = mapper;
        _blobStorage = blobStorage;
    }


    public async Task<string> UploadFileAsync(int userId, IFormFile file)
    {
        var user = await _userRepository.GetUserById<User>(userId);
        
        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{originalFileName}_{Guid.NewGuid()}{extension}";

        var fileUrl = await _blobStorage.UploadAsync(uniqueFileName,file);
        
        var fileRecord = new FileRecord
        {
            fileName = uniqueFileName,
            filePath = fileUrl,
            fileSize = file.Length,
            fileType = file.ContentType,
            userId = user.userId
        };

        await _fileRepository.AddFileAsync(fileRecord);
        user.files.Add(fileRecord);
        var result = await _userRepository.SaveChangesAsync(user);
        Console.WriteLine(result);

        return fileUrl;
    }

    public async Task<bool> DeleteFileAsync(int fileId)
    {
        var file = await _fileRepository.GetFileByIdAsync(fileId);
        var user = await _userRepository.GetUserById<User>(file.userId);

        var resultAzure = await _blobStorage.DeleteAsync(file.fileName);
        var resultFile = await _fileRepository.DeleteFileAsync(file);
        var resultUser = await _userRepository.DeleteFileFromUserASync(user, file);
        return resultFile && resultUser && resultAzure;
    }

    public async Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId)
    {
        var files = await _fileRepository.GetAllFilesAsync();
        var result = files.Where(f => f.userId == userId).ToList();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<IEnumerable<FileDTOs>> GetAllFilesAsync()
    {
        var result = await _fileRepository.GetAllFilesAsync();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<(Stream?, string fileName)?> DownloadFileAsync(int fileId)
    {
        var file = await _fileRepository.GetFileByIdAsync(fileId);
        var stream = await _blobStorage.DownloadAsync(file.fileName);
        return (stream, file.fileName);
    }
}