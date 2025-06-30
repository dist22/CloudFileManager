using AutoMapper;
using Cloud.DTOs.User;
using Cloud.Exceptions;
using Cloud.Models;
using Cloud.Interfaces.BlobStorage;
using Cloud.Interfaces.Repositories;
using Cloud.Interfaces.Services;

namespace Cloud.Services;

public class UserServices : BaseServices, IUserServices
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;

    public UserServices(IBlobStorage blobStorage, IMapper mapper, IBaseRepository<User> userRepository)
    {
        _blobStorage = blobStorage;
        _mapper = mapper;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDTOs>> GetUsersAsync()
    {
        var users = await _userRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<UserDTOs>>(users);
    }

    public async Task<UserDTOs> GetUserAsync(int userId)
    {
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.userId == userId));
        return _mapper.Map<UserDTOs>(user);
    }

    public async Task EditUserAsync(int userId, string role)
    {
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.userId == userId));
        user.role = role;
        user.updateAt = DateTime.Now;
        if (! await _userRepository.Update(user))
            throw new ConflictException("Couldn't update user");
    }

    public async Task DeleteUserAsync(int userId)
    {
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.userId == userId));
        await _blobStorage.DeleteUserContainerAsync(user);
        if (!await _userRepository.Remove(user))
            throw new ConflictException("Couldn't delete user");
    }
}