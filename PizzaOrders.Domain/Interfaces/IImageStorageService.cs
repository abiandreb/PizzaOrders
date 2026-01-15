namespace PizzaOrders.Domain.Interfaces;

public interface IImageStorageService
{
    /// <summary>
    /// Uploads an image in three sizes (thumbnail, medium, full) to blob storage.
    /// </summary>
    /// <param name="productId">The product ID for organizing images</param>
    /// <param name="imageStream">The image data stream</param>
    /// <param name="fileName">The original file name</param>
    /// <param name="contentType">The content type (e.g., image/jpeg, image/png)</param>
    /// <returns>The URLs for thumbnail, medium, and full-size images</returns>
    Task<ProductImageUrls> UploadProductImageAsync(int productId, Stream imageStream, string fileName, string contentType);

    /// <summary>
    /// Retrieves an image from blob storage.
    /// </summary>
    /// <param name="blobName">The blob name/path</param>
    /// <returns>The image stream and content type</returns>
    Task<(Stream ImageStream, string ContentType)> GetImageAsync(string blobName);

    /// <summary>
    /// Deletes all images (thumbnail, medium, full) for a product.
    /// </summary>
    /// <param name="productId">The product ID</param>
    Task DeleteProductImagesAsync(int productId);

    /// <summary>
    /// Checks if an image exists in blob storage.
    /// </summary>
    /// <param name="blobName">The blob name/path</param>
    /// <returns>True if the image exists, otherwise false</returns>
    Task<bool> ImageExistsAsync(string blobName);
}

public record ProductImageUrls(
    string ThumbnailUrl,
    string MediumUrl,
    string FullUrl
);
