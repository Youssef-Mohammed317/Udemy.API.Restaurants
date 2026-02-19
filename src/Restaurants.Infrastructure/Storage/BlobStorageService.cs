using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Extensions.Options;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Infrastructure.Storage;

public class BlobStorageSettings
{
    public string ConnectionString { get; set; } = default!;
    public string ContainerName { get; set; } = default!;
    public string AccountKey { get; set; } = default!;
}
public class BlobStorageService : IBlobStorageService
{
    private readonly BlobStorageSettings _settings;

    public BlobStorageService(IOptions<BlobStorageSettings> options)
    {
        _settings = options.Value;
    }

    public async Task<string> UploadToBlobAsync(Stream file, string fileName)
    {
        var blobServiceClient = new BlobServiceClient(_settings.ConnectionString);
        var containerClient = blobServiceClient.GetBlobContainerClient(_settings.ContainerName);

        var blobClient = containerClient.GetBlobClient(fileName);

        await blobClient.UploadAsync(file, overwrite: true);
        return blobClient.Uri.ToString();
    }

    public string? GetBlobSasUrl(string? blobUrl)
    {
        if (blobUrl == null) return null;
        var sasBuilder = new BlobSasBuilder
        {
            BlobContainerName = _settings.ContainerName,
            BlobName = blobUrl.Split('/').Last(),
            Resource = "b",
            StartsOn = DateTimeOffset.UtcNow,
            ExpiresOn = DateTimeOffset.UtcNow.AddHours(1)
        };
        sasBuilder.SetPermissions(BlobSasPermissions.Read);
        var sasToken = sasBuilder.ToSasQueryParameters(new Azure.Storage.StorageSharedKeyCredential(
            new BlobServiceClient(_settings.ConnectionString).AccountName,
            _settings.AccountKey
        )).ToString();
        return $"{blobUrl}?{sasToken}";
    }
}
