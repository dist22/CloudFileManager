using AutoMapper;
using Cloud.DTOs.User;
using Cloud.Interfaces;
using Cloud.Interfaces.BlobStorage;
using Cloud.Interfaces.PasswordHasher;
using Cloud.Interfaces.Repositories;
using Cloud.Interfaces.Services;
using Cloud.Models;

namespace Cloud.Services;

public class AuthServices : BaseServices, IAuthServices
{
    private readonly IBaseRepository<User> _userRepository;
    private readonly IBlobStorage _blobStorage;
    private readonly IMapper _mapper;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IJwtProvider _jwtProvider;

    public AuthServices(IBlobStorage blobStorage, IMapper mapper, IPasswordHasher passwordHasher,
        IJwtProvider jwtProvider, IBaseRepository<User> userRepository)
    {
        _blobStorage = blobStorage;
        _mapper = mapper;
        _passwordHasher = passwordHasher;
        _jwtProvider = jwtProvider;
        _userRepository = userRepository;
    }


    public async Task<bool> RegisteredAsync(UserCreateDTO userCreateDto)
    {
        if (!await IfExistAsync(userCreateDto.email, userCreateDto.username))
        {
            if (await PasswordConfirmAsync(userCreateDto.password, userCreateDto.passwordConfirm))
            {
                var user = await _userRepository.AddAsync(_mapper.Map<User>(userCreateDto));
                user.containerName = await _blobStorage.CreateUserContainerAsync(user.userId.ToString());
                user.password = _passwordHasher.Generate(userCreateDto.password);
                return await _userRepository.Update(user);
            }

            throw new Exception("Password don`t match");
        }

        throw new Exception("User already exists");
    }

    public async Task<string> LoginAsync(UserLoginDTO userLoginDto)
    {
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.email == userLoginDto.email));
        if (_passwordHasher.Verify(userLoginDto.password, user.password))
            return _jwtProvider.CreateToken(user);
        throw new Exception("error");
    }

    private Task<bool> PasswordConfirmAsync(string password, string passwordConfirm)
        => Task.FromResult(password == passwordConfirm);

    private async Task<bool> IfExistAsync(string email, string username)
        => await _userRepository.IfExistAsync(u => u.email == email) &&
           await _userRepository.IfExistAsync(u => u.email == username);
}