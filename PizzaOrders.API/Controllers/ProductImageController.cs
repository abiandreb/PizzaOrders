using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PizzaOrders.Application.DTOs;
using PizzaOrders.Domain.Entities.Products;
using PizzaOrders.Domain.Interfaces;
using PizzaOrders.Infrastructure.Data;

namespace PizzaOrders.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductImageController : ControllerBase
{
    private readonly IImageStorageService _imageStorageService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<ProductImageController> _logger;

    public ProductImageController(
        IImageStorageService imageStorageService,
        AppDbContext dbContext,
        ILogger<ProductImageController> logger)
    {
        _imageStorageService = imageStorageService;
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Upload an image for a product. Creates thumbnail, medium, and full-size versions.
    /// </summary>
    [HttpPost("upload/{productId:int}")]
    [Authorize(Roles = "Admin")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ImageUploadResponseDto>> UploadProductImage(
        int productId,
        IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // Validate file type
        var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/webp" };
        if (!allowedContentTypes.Contains(file.ContentType.ToLower()))
        {
            return BadRequest("Only JPEG, PNG, and WebP images are allowed.");
        }

        // Validate file size (max 10MB)
        const long maxFileSize = 10 * 1024 * 1024;
        if (file.Length > maxFileSize)
        {
            return BadRequest("File size must not exceed 10MB.");
        }

        // Check if product exists
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound($"Product with ID {productId} not found.");
        }

        try
        {
            // Upload images to Azurite
            using var stream = file.OpenReadStream();
            var imageUrls = await _imageStorageService.UploadProductImageAsync(
                productId,
                stream,
                file.FileName,
                file.ContentType);

            // Update product entity with image URLs
            if (product.ProductImage == null)
            {
                product.ProductImage = ProductImage.Empty();
            }

            product.ProductImage.UpdateUrls(
                imageUrls.ThumbnailUrl,
                imageUrls.MediumUrl,
                imageUrls.FullUrl);

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation(
                "Successfully uploaded images for product {ProductId}. Thumbnail: {ThumbnailUrl}",
                productId,
                imageUrls.ThumbnailUrl);

            return Ok(new ImageUploadResponseDto(
                imageUrls.ThumbnailUrl,
                imageUrls.MediumUrl,
                imageUrls.FullUrl));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to upload image for product {ProductId}", productId);
            return StatusCode(500, "An error occurred while uploading the image.");
        }
    }

    /// <summary>
    /// Get image URLs for a specific product.
    /// </summary>
    [HttpGet("{productId:int}")]
    public async Task<ActionResult<ProductImageResponseDto>> GetProductImages(int productId)
    {
        var product = await _dbContext.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return NotFound($"Product with ID {productId} not found.");
        }

        return Ok(new ProductImageResponseDto(
            product.ProductImage?.ThumbnailUrl,
            product.ProductImage?.MediumUrl,
            product.ProductImage?.FullUrl));
    }

    /// <summary>
    /// Delete all images for a product.
    /// </summary>
    [HttpDelete("{productId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProductImages(int productId)
    {
        var product = await _dbContext.Products.FindAsync(productId);
        if (product == null)
        {
            return NotFound($"Product with ID {productId} not found.");
        }

        try
        {
            // Delete from blob storage
            await _imageStorageService.DeleteProductImagesAsync(productId);

            // Clear image URLs from database
            if (product.ProductImage != null)
            {
                product.ProductImage.Clear();
            }

            await _dbContext.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted images for product {ProductId}", productId);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete images for product {ProductId}", productId);
            return StatusCode(500, "An error occurred while deleting the images.");
        }
    }

    /// <summary>
    /// Download a specific image by blob name (for internal use or debugging).
    /// </summary>
    [HttpGet("download")]
    public async Task<ActionResult> DownloadImage([FromQuery] string blobName)
    {
        if (string.IsNullOrWhiteSpace(blobName))
        {
            return BadRequest("Blob name is required.");
        }

        try
        {
            var (imageStream, contentType) = await _imageStorageService.GetImageAsync(blobName);
            return File(imageStream, contentType);
        }
        catch (FileNotFoundException)
        {
            return NotFound($"Image not found: {blobName}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to download image {BlobName}", blobName);
            return StatusCode(500, "An error occurred while downloading the image.");
        }
    }
}
