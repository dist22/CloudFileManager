using AutoMapper;
using Cloud.Interfaces;
using Cloud.Models;
using Cloud.DTOs;

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

        var fileUrl = await _blobStorage.UploadAsync(file);

        var fileRecord = new FileRecord
        {
            fileName = file.FileName,
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

    public async Task<bool> DeleteFileAsync(int userId, int fileId)
    {
        var file = await _fileRepository.GetFileByIdAsync(fileId);
        var resultFile=await _fileRepository.DeleteFileAsync(file);
        var user = await _userRepository.GetUserById<User>(userId);
        var userResult = await _userRepository.DeleteFileFromUserASync(user, file);
        return resultFile && userResult;
    }

    public async Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId)
    {
        var files = await _fileRepository.GetAllFilesAsync();
        var result= files.Where(f => f.userId == userId).ToList();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<IEnumerable<FileDTOs>> GetAllFilesAsync()
    {
        var result = await _fileRepository.GetAllFilesAsync();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }
}