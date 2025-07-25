﻿using AutoMapper;
using Cloud.Application.DTOs.User;
using Cloud.Application.Interfaces.Services;
using Cloud.Domain.Exceptions;
using Cloud.Domain.Interfaces.BlobStorage;
using Cloud.Domain.Interfaces.JwtProvider;
using Cloud.Domain.Interfaces.PasswordHasher;
using Cloud.Domain.Interfaces.Repositories;
using Cloud.Domain.Models;

namespace Cloud.Application.Services;

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


    public async Task RegisteredAsync(UserCreateDTO userCreateDto)
    {
        if (!await IfExistAsync(userCreateDto.email, userCreateDto.username))
        {
            if (await PasswordConfirmAsync(userCreateDto.password, userCreateDto.passwordConfirm))
            {
                var user = await _userRepository.AddAsync(_mapper.Map<User>(userCreateDto));
                user.containerName = await _blobStorage.CreateUserContainerAsync(user.userId.ToString());
                user.password = _passwordHasher.Generate(userCreateDto.password);
                if (!await _userRepository.Update(user))
                    throw new ConflictException("Coudn`t registered user");
            }
            throw new ConflictException("Password don`t match");
        }
        throw new ConflictException("User already exists");
    }

    public async Task<string> LoginAsync(UserLoginDTO userLoginDto)
    {
        var user = await GetIfNotNullAsync(_userRepository.GetAsync(u => u.email == userLoginDto.email));
        if (_passwordHasher.Verify(userLoginDto.password, user.password))
            return _jwtProvider.CreateToken(user);
        throw new ConflictException("Uncorect password");
    }

    private Task<bool> PasswordConfirmAsync(string password, string passwordConfirm)
        => Task.FromResult(password == passwordConfirm);

    private async Task<bool> IfExistAsync(string email, string username)
        => await _userRepository.IfExistAsync(u => u.email == email) &&
           await _userRepository.IfExistAsync(u => u.email == username);
}