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

    public async Task<string> UploadAsync(string uniqueFileName, IFormFile file)
    {
        var blobClient = _client.GetBlobClient(uniqueFileName);
        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: false);
        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteAsync(string fileName)
    {
        var blobClient = _client.GetBlobClient(fileName);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<Stream?> DownloadAsync(string fileName)
    {
        var blobClient = _client.GetBlobClient(fileName);
        var exists = await blobClient.ExistsAsync();
        return exists ? (await blobClient.DownloadAsync()).Value.Content : 
            throw new Exception("Error");
    }
}