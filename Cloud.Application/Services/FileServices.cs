using AutoMapper;
using Cloud.Application.DTOs.File;
using Cloud.Application.Interfaces.Services;
using Cloud.Domain.Exceptions;
using Cloud.Domain.Interfaces.FileSizeConverter;
using Cloud.Domain.Interfaces.Repositories;
using Cloud.Domain.Interfaces.BlobStorage;
using Cloud.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Cloud.Application.Services;

public class FileServices : BaseServices, IFileServices
{
    private readonly IBaseRepository<FileRecord> _fileRepository;
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IFileSizeConverter _fileSizeConverter;
    private readonly IMapper _mapper;

    public FileServices(IMapper mapper, IBlobStorage blobStorage, IFileSizeConverter fileSizeConverter,
        IBaseRepository<FileRecord> fileRepository, IBaseRepository<User> userRepository)
    {
        _mapper = mapper;
        _blobStorage = blobStorage;
        _fileSizeConverter = fileSizeConverter;
        _fileRepository = fileRepository;
        _userRepository = userRepository;
    }


    public async Task<string> UploadFileAsync(int userId, IFormFile file)
    {
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.userId == userId));

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

        if(await _fileRepository.AddAsync(fileRecord) == null)
            throw new ConflictException("Couldn't add file");

        return fileRecord.filePath;
    }

    public async Task DeleteAnyFileAsync(int fileId)
    {
        var (file, user) = await GetFileWithUserAsync(fileId);
        await TryDeleteFileOrThrowAsync(file, user);
    }

    public async Task DeleteUserFileAsync(int userId, int fileId)
    {
        var(file, user) = await GetValidatedFileAccess(fileId, userId);
        await TryDeleteFileOrThrowAsync(file, user);
    }

    public async Task<FileDTOs> GetFileByIdAsync(int fileId)
        => _mapper.Map<FileDTOs>(
            await GetIfNotNullAsync(_fileRepository.GetAsync(f => f.fileId == fileId)));

    public async Task<FileDTOs> GetMyFileAsync(int fileId, int fileOwnerId)
    {
        var (file, user) = await GetValidatedFileAccess(fileId, fileOwnerId);
        return _mapper.Map<FileDTOs>(file);
    }


    public async Task<IEnumerable<FileDTOs>> GetAllUserFilesAsync(int userId)
    {
        var files = await _fileRepository.GetAllAsync();
        var result = files.Where(f => f.userId == userId).ToList();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<IEnumerable<FileDTOs>> GetAllFilesAsync()
    {
        var result = await _fileRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<FileDTOs>>(result);
    }

    public async Task<(Stream?, string fileName)?> DownloadFileAsync(int fileId)
    {
        var (file, user) = await GetFileWithUserAsync(fileId);
        return (await _blobStorage.DownloadAsync(file, user), file.fileName);
    }

    public async Task<(Stream?, string fileName)?> DownloadMyFileAsync(int userId, int fileId)
    {
        var(file, user) = await GetValidatedFileAccess(fileId, userId);
        return (await _blobStorage.DownloadAsync(file, user), file.fileName);
    }

    private async Task<(FileRecord file, User user)> GetFileWithUserAsync(int fileId)
    {
        var file = await GetIfNotNullAsync(_fileRepository.GetAsync(f => f.fileId == fileId));
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.userId == file.userId));

        return (file, user);
    }

    private async Task<(FileRecord file, User user)> GetValidatedFileAccess(int fileId, int fileOwnerId)
    {
        var file = await GetIfNotNullAsync(_fileRepository.GetAsync(f => f.fileId == fileId));

        if (file.userId != fileOwnerId)
            throw new ForbiddenException($"You don`t have permissions to file-{fileId}");
        
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.userId == fileOwnerId));
        return (file, user);
    }

    private async Task TryDeleteFileOrThrowAsync(FileRecord file, User user)
    {
        if(!(await _fileRepository.Remove(file)
             && await _blobStorage.DeleteAsync(file, user)))
            throw new ConflictException("Couldn't delete file");
    }
}