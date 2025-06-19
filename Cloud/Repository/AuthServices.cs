using AutoMapper;
using Cloud.DTOs;
using Cloud.Interfaces;
using Cloud.Models;

namespace Cloud.Repository;

public class AuthServices : IAuthServices
{
    private readonly IUserRepository _userRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;

    public AuthServices(IUserRepository userRepository, IBlobStorage blobStorage, IMapper mapper,
        IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _blobStorage = blobStorage;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
    }


    public async Task<bool> RegisteredAsync(UserCreateDTO userCreateDto)
    {
        if (!await IfExistAsync(userCreateDto.email, userCreateDto.username))
        {
            if (await PasswordConfirmAsync(userCreateDto.password, userCreateDto.passwordConfirm))
            {
                var user = await _userRepository.AddUserAsync((_mapper.Map<User>(userCreateDto)));
                user.containerName = await _blobStorage.CreateUserContainerAsync(user.userId.ToString());
                user.password = _passwordHasher.Generate(userCreateDto.password);
                return await _userRepository.EditUser(user);
            }

            throw new Exception("Password don`t match");
        }
        throw new Exception("User already exists");
    }

    public async Task<bool> LoginAsync(UserLoginDTO userLoginDto)
    {
        var user = await _userRepository.GetUser(u => u.email == userLoginDto.email);
        if (_passwordHasher.Verify(userLoginDto.password, user.password))
            return true;
        throw new Exception("error");
    }

    public Task<bool> PasswordConfirmAsync(string password, string passwordConfirm)
        => Task.FromResult(password == passwordConfirm);

    public async Task<bool> IfExistAsync(string email, string username)
        => await _userRepository.UserExists(u => u.email == email) &&
           await _userRepository.UserExists(u => u.email == username);
}