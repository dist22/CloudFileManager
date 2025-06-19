using AutoMapper;
using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Models;

namespace Cloud.Repository;

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

    // public async Task<bool> CreateUserAsync(UserCreateDTO userCreateDto)
    // {
    //     var exist = await _userRepository.UserExists(u => u.email == userCreateDto.email) &&
    //                 await _userRepository.UserExists(u => u.username == userCreateDto.username);
    //     if (exist)
    //         throw new Exception("User already exists");
    //     
    //     var user = await _userRepository.AddUserAsync((_mapper.Map<User>(userCreateDto)));
    //     user.containerName = await _blobStorage.CreateUserContainerAsync(user.userId.ToString());
    //     
    //     return await _userRepository.EditUser(user);
    // }

    public async Task<bool> EditUserAsync(int userId, UserEditDTO userEditDto)
    {
        var user = await _userRepository.GetUser(u => u.userId == userId);
        user.username = userEditDto.username;
        user.email = userEditDto.email;
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