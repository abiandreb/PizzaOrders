using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using PizzaOrders.Domain.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace PizzaOrders.Infrastructure.Services;

/// <summary>
/// Implementation of image storage using Azure Blob Storage (Azurite for local development).
/// Stores product images in three sizes: thumbnail (150x150), medium (500x500), and full (original).
/// </summary>
public class AzuriteImageStorageService : IImageStorageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private const int ThumbnailSize = 150;
    private const int MediumSize = 500;

    public AzuriteImageStorageService(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("AzuriteStorage")
            ?? throw new InvalidOperationException("Azurite storage connection string not found.");

        _containerName = configuration["AzuriteStorage:ContainerName"] ?? "product-images";

        _blobServiceClient = new BlobServiceClient(connectionString);

        // Ensure container exists
        InitializeContainerAsync().GetAwaiter().GetResult();
    }

    private async Task InitializeContainerAsync()
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
    }

    public async Task<ProductImageUrls> UploadProductImageAsync(
        int productId,
        Stream imageStream,
        string fileName,
        string contentType)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        // Reset stream position
        imageStream.Position = 0;

        // Load the original image
        using var image = await Image.LoadAsync(imageStream);

        // Generate file extension
        var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(fileExtension))
        {
            fileExtension = ".jpg";
        }

        // Upload full-size image
        var fullBlobName = $"{productId}/full{fileExtension}";
        var fullUrl = await UploadImageVariantAsync(containerClient, fullBlobName, image, null, contentType);

        // Upload medium-size image
        var mediumBlobName = $"{productId}/medium{fileExtension}";
        var mediumUrl = await UploadImageVariantAsync(containerClient, mediumBlobName, image, MediumSize, contentType);

        // Upload thumbnail image
        var thumbnailBlobName = $"{productId}/thumbnail{fileExtension}";
        var thumbnailUrl = await UploadImageVariantAsync(containerClient, thumbnailBlobName, image, ThumbnailSize, contentType);

        return new ProductImageUrls(thumbnailUrl, mediumUrl, fullUrl);
    }

    private async Task<string> UploadImageVariantAsync(
        BlobContainerClient containerClient,
        string blobName,
        Image image,
        int? targetSize,
        string contentType)
    {
        var blobClient = containerClient.GetBlobClient(blobName);

        using var outputStream = new MemoryStream();

        if (targetSize.HasValue)
        {
            // Resize image while maintaining aspect ratio
            var resizedImage = image.Clone(ctx => ctx.Resize(new ResizeOptions
            {
                Size = new Size(targetSize.Value, targetSize.Value),
                Mode = ResizeMode.Max
            }));

            await resizedImage.SaveAsync(outputStream, new JpegEncoder { Quality = 85 });
        }
        else
        {
            // Save original size
            await image.SaveAsync(outputStream, new JpegEncoder { Quality = 90 });
        }

        outputStream.Position = 0;

        var blobHttpHeaders = new BlobHttpHeaders
        {
            ContentType = contentType
        };

        await blobClient.UploadAsync(outputStream, new BlobUploadOptions
        {
            HttpHeaders = blobHttpHeaders
        });

        return blobClient.Uri.ToString();
    }

    public async Task<(Stream ImageStream, string ContentType)> GetImageAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        if (!await blobClient.ExistsAsync())
        {
            throw new FileNotFoundException($"Image not found: {blobName}");
        }

        var download = await blobClient.DownloadAsync();
        var contentType = download.Value.ContentType;

        var memoryStream = new MemoryStream();
        await download.Value.Content.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return (memoryStream, contentType);
    }

    public async Task DeleteProductImagesAsync(int productId)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

        // Delete all blobs with the productId prefix
        var prefix = $"{productId}/";

        await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
        {
            var blobClient = containerClient.GetBlobClient(blobItem.Name);
            await blobClient.DeleteIfExistsAsync();
        }
    }

    public async Task<bool> ImageExistsAsync(string blobName)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobName);

        return await blobClient.ExistsAsync();
    }
}
