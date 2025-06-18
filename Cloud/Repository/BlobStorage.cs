using Cloud.Interfaces;
using Cloud.Models;
using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Cloud.Repository;

public class BlobStorage : IBlobStorage
{
    private readonly BlobServiceClient _client;

    public BlobStorage(BlobServiceClient client)
    {
        _client = client;
    }

    public async Task<string> CreateUserContainerAsync(string userId)
    {
        var containerName = $"user-{userId}-{Guid.NewGuid()}".ToLower();
        var containerClient = _client.GetBlobContainerClient(containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.None);
        return containerName;
    }

    public async Task<bool> DeleteUserContainerAsync(User user)
    {
        return await _client.GetBlobContainerClient(user.containerName).DeleteIfExistsAsync();
    }

    public async Task<string> UploadAsync(string uniqueFileName, IFormFile file, User user)
    {

        var blobClient = _client.GetBlobContainerClient(user.containerName).GetBlobClient(uniqueFileName);
        using var stream = file.OpenReadStream();
        await blobClient.UploadAsync(stream, overwrite: false);
        return blobClient.Uri.ToString();
    }

    public async Task<bool> DeleteAsync(FileRecord file, User user)
    {
        var blobClient = _client.GetBlobContainerClient(user.containerName).GetBlobClient(file.fileName);
        return await blobClient.DeleteIfExistsAsync();
    }

    public async Task<Stream?> DownloadAsync(FileRecord file, User user)
    {
        var blobClient = _client.GetBlobContainerClient(user.containerName).GetBlobClient(file.fileName);
        var exists = await blobClient.ExistsAsync();
        return exists ? (await blobClient.DownloadAsync()).Value.Content : 
            throw new Exception("Error");
    }
}