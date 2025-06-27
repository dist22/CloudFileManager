using AutoMapper;
using Cloud.Models;
using Cloud.DTOs.File;
using Cloud.Interfaces.BlobStorage;
using Cloud.Interfaces.FileSizeConverter;
using Cloud.Interfaces.Repositories;
using Cloud.Interfaces.Services;

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
        _mapper = mapper;
        _blobStorage = blobStorage;
        _fileSizeConverter = fileSizeConverter;
    }


    public async Task<string> UploadFileAsync(int userId, IFormFile file)
    {
        var user = await _userRepository.GetUserIfExistAsync(u => u.userId == userId);

        var originalFileName = Path.GetFileNameWithoutExtension(file.FileName);
        var extension = Path.GetExtension(file.FileName);
        var uniqueFileName = $"{originalFileName}_{Guid.NewGuid()}{extension}";
        
        var fileRecord = new FileRecord
        {
            fileName = uniqueFileName,
            filePath = await _blobStorage.UploadAsync(uniqueFileName, file, user),
            fileSize = _fileSizeConverter.FormatSize(file.Length),
            fileType = file.ContentType,
            userId = user.userId
        };

        await _fileRepository.AddFileAsync(fileRecord);
        user.files.Add(fileRecord);
        await _userRepository.EditUser(user);

        return fileRecord.filePath;
    }

    public async Task<bool> DeleteAnyFileAsync(int fileId)
    {
        var file = await _fileRepository.GetFileIfExistAsync(fileId);
        var user = await _userRepository.GetUserIfExistAsync(u => u.userId == file.userId);
        
        return await _blobStorage.DeleteAsync(file, user) 
               && await _fileRepository.DeleteFileAsync(file);
    }

    public async Task<bool> DeleteUserFileAsync(int userId, int fileId)
    {
       var file = await _fileRepository.GetFileIfExistAsync(fileId);
       var user = await _userRepository.GetUserIfExistAsync(u => u.userId == userId);
       
       if (file.userId != userId)
           return false;
       
       return await _fileRepository.DeleteFileAsync(file) 
              && await _blobStorage.DeleteAsync(file, user);
    }

    public async Task<FileDTOs> GetFileByIdAsync(int fileId) 
        => _mapper.Map<FileDTOs>(await _fileRepository.GetFileIfExistAsync(fileId));

    public async Task<FileDTOs> GetMyFileAsync(int fileId, int fileOwnerId)
    {
        var file = await _fileRepository.GetFileIfExistAsync(fileId);
        if (file.userId != fileOwnerId)
            throw new UnauthorizedAccessException();
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
        var file = await _fileRepository.GetFileIfExistAsync(fileId);
        var user = await _userRepository.GetUserIfExistAsync(u => u.userId == file.userId);
        
        return (await _blobStorage.DownloadAsync(file, user), file.fileName);
    }

    public async Task<(Stream?, string fileName)?> DownloadMyFileAsync(int userId, int fileId)
    {
        var file = await _fileRepository.GetFileIfExistAsync(fileId);
        var user = await _userRepository.GetUserIfExistAsync(u => u.userId == userId);
      
        if (file.userId != userId)
            throw new UnauthorizedAccessException();

        return (await _blobStorage.DownloadAsync(file, user), file.fileName);
    }
}