using System.Linq.Expressions;
using Cloud.Interfaces.Repositories;

namespace Cloud.Services;

public  abstract class BaseServices()
{
    protected async Task<T> GetIfNotNullAsync<T>(Task<T> task)
    {
        var result = await task;
        if(result == null)
            throw new NullReferenceException();
        return result;
    }
}