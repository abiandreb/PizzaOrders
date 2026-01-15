namespace PizzaOrders.Domain.Entities.Products;

/// <summary>
/// Value object representing product image URLs in different sizes.
/// Stored as an owned entity in ProductEntity.
/// </summary>
public class ProductImage
{
    public string? ThumbnailUrl { get; private set; }
    public string? MediumUrl { get; private set; }
    public string? FullUrl { get; private set; }

    private ProductImage() { }

    public ProductImage(string? thumbnailUrl, string? mediumUrl, string? fullUrl)
    {
        ThumbnailUrl = thumbnailUrl;
        MediumUrl = mediumUrl;
        FullUrl = fullUrl;
    }

    public static ProductImage Create(string thumbnailUrl, string mediumUrl, string fullUrl)
    {
        return new ProductImage(thumbnailUrl, mediumUrl, fullUrl);
    }

    public static ProductImage Empty()
    {
        return new ProductImage(null, null, null);
    }

    public bool HasImages()
    {
        return !string.IsNullOrWhiteSpace(ThumbnailUrl) ||
               !string.IsNullOrWhiteSpace(MediumUrl) ||
               !string.IsNullOrWhiteSpace(FullUrl);
    }

    public void UpdateUrls(string thumbnailUrl, string mediumUrl, string fullUrl)
    {
        ThumbnailUrl = thumbnailUrl;
        MediumUrl = mediumUrl;
        FullUrl = fullUrl;
    }

    public void Clear()
    {
        ThumbnailUrl = null;
        MediumUrl = null;
        FullUrl = null;
    }
}
