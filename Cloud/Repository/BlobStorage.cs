using Cloud.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Cloud.Repository;

public class BlobStorage : IBlobStorage
{

    private readonly BlobContainerClient _client;
    
    public BlobStorage(IConfiguration configuration)
    {
        var connectionString = configuration["Azure:ConnectionString"];
        var container = configuration["Azure:Container"];
        _client = new BlobContainerClient(connectionString, container);
    }

    public async Task<string> UploadAsync(IFormFile file)
    {
        var blobClient = _client.GetBlobClient(file.FileName);
        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: true);
        return blobClient.Uri.ToString();
    }
    
    public async Task<bool> DeleteAsync(string fileName)
    {
        var blobClient = _client.GetBlobClient(fileName);
        return await blobClient.DeleteIfExistsAsync();
    }

}

