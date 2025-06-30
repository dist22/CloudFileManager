using System.Linq.Expressions;
using Cloud.Exceptions;
using Cloud.Interfaces.Repositories;

namespace Cloud.Services;

public  abstract class BaseServices()
{
    protected async Task<T> GetIfNotNullAsync<T>(Task<T> task)
    {
        var result = await task;
        if(result == null)
            throw new NotFoundException("Object with this params not foud");
        return result;
    }
}