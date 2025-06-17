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
        var user = await _userRepository.GetUser(userId);
        return _mapper.Map<UserDTOs>(user);
    }

    public async Task<bool> CreateUserAsync(UserForCreate userForCreate)
    {
        var exist = await _userRepository.UserExists(u => u.email == userForCreate.email) &&
                    await _userRepository.UserExists(u => u.username == userForCreate.username);
        if (exist)
            throw new Exception("User already exists");
        var result = await _userRepository.AddUserAsync(_mapper.Map<User>(userForCreate));
        return result;
    }

    public async Task<bool> EditUserAsync(int userId, UserForEdit userForEdit)
    {
        var user = await _userRepository.GetUser(userId);
        user.username = userForEdit.username;
        user.email = userForEdit.email;
        user.role = userForEdit.role;
        return await _userRepository.EditUser(user);
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var user = await _userRepository.GetUser(userId);
        foreach (var file in user.files.ToList())
        {
            await _blobStorage.DeleteAsync(file.fileName);
            await _fileRepository.DeleteFileAsync(file);
        }
        return await _userRepository.DeleteUser(user);
    }
}