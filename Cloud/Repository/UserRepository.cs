using AutoMapper;
using Cloud.Data;
using Cloud.Interfaces;
using Cloud.DTOs;
using Cloud.Models;
using Microsoft.EntityFrameworkCore;

namespace Cloud.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContextEF _entity;
    private readonly IMapper _mapper;

    public UserRepository(DataContextEF entity, IMapper mapper)
    {
        _entity = entity;
        _mapper = mapper;
    }


    public async Task<IEnumerable<User>> GetListOfUsers()
    {
        return await _entity.Users
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _entity.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.userId == id);
        if (user != null)
            return user;
        throw new Exception("User with this id is not found");

    }

    public async Task<bool> GetUserByEmail(string email)
    {
        User? user = await _entity.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.email == email);

        if (user != null)
        {
            return true;
        }
        return false;
    }
    
    public async Task<bool> GetUserByUserName(string userName)
    {
        User? user = await _entity.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.email == userName);

        if (user != null)
        {
            return true;
        }
        return false;
    }

    public async Task CreateUser(UserForCreate userForCreate)
    {
        User user = new User
        {
            email = userForCreate.email,
            username = userForCreate.username
        };
        await _entity.AddAsync(user);
        if (!(await _entity.SaveChangesAsync() > 0))
            throw new Exception("Error");
    }

    public async Task EditUser(User user, UserForEdit userForEdit)
    {
        user.username = userForEdit.username;
        user.email = userForEdit.email;
        user.role = userForEdit.role;
        user.updateAt = DateTime.Now;
        _entity.Users.Update(user);
        if (!(await _entity.SaveChangesAsync() > 0))
            throw new Exception("Error");
    }

    public async Task DeleteUser(User user)
    {
        _entity.Users.Remove(user);
        if (! (await _entity.SaveChangesAsync() > 0))
            throw new Exception("Error");
    }


}