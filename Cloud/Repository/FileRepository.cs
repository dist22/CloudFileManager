using Cloud.Data;
using Cloud.Interfaces;

namespace Cloud.Repository;

public class FileRepository : IFileRepository
{
    private readonly DataContextEF _entity;

    public FileRepository(DataContextEF entity)
    {
        _entity = entity;
    }
}