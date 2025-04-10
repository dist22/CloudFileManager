using Cloud.Data;
using Cloud.Interfaces;

namespace Cloud.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContextEF _entity;

    public UserRepository(DataContextEF entity)
    {
        _entity = entity;
    }
}