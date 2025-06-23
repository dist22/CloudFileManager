using AutoMapper;
using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Models;

namespace Cloud.Services;

public class UserServices : IUserServices
{
    private readonly IUserRepository _userRepository;
    private readonly IFileRepository _fileRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public UserServices(IUserRepository userRepository, IFileRepository fileRepository, IBlobStorage blobStorage,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _fileRepository = fileRepository;
        _blobStorage = blobStorage;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDTOs>> GetUsersAsync()
    {
        var users = await _userRepository.GetUsers();
        return _mapper.Map<IEnumerable<UserDTOs>>(users);
    }

    public async Task<UserDTOs> GetUserAsync(int userId)
    {
        var user = await _userRepository.GetUser(u => u.userId == userId);
        return _mapper.Map<UserDTOs>(user);
    }
    
    public async Task<bool> EditUserAsync(UserEditDTO userEditDto)
    {
        var user = await _userRepository.GetUser(u => u.userId == userEditDto.userId);
        user.role = userEditDto.role;
        user.updateAt = DateTime.Now;
        return await _userRepository.EditUser(user);
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetUser(u => u.userId ==userId);
        foreach (var file in user.files.ToList())
        {
            await _fileRepository.DeleteFileAsync(file);
        }
        await _blobStorage.DeleteUserContainerAsync(user);
        return await _userRepository.DeleteUser(user);
    }
}