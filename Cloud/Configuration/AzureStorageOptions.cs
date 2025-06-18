using Azure.Storage.Blobs;

namespace Cloud.Configuration;

public class AzureStorageOptions
{
    public string ConnectionString { get; set; } = String.Empty;
}