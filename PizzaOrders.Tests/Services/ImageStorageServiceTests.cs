using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using PizzaOrders.Application.Interfaces;
using Xunit;

namespace PizzaOrders.Tests.Services;

public class ImageStorageServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;
    private readonly string _testConnectionString;
    private readonly string _testContainerName;

    public ImageStorageServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
        _testContainerName = "test-product-images";
        _testConnectionString = "UseDevelopmentStorage=true;";

        var mockConnectionSection = new Mock<IConfigurationSection>();
        mockConnectionSection.Setup(x => x.Value).Returns(_testConnectionString);
        _mockConfiguration.Setup(x => x.GetConnectionString("AzuriteStorage")).Returns(_testConnectionString);

        var mockContainerSection = new Mock<IConfigurationSection>();
        mockContainerSection.Setup(x => x.Value).Returns(_testContainerName);
        _mockConfiguration.Setup(x => x["AzuriteStorage:ContainerName"]).Returns(_testContainerName);
    }

    [Fact]
    public void ProductImageUrls_ShouldInitializeCorrectly()
    {
        // Arrange
        var thumbnailUrl = "https://example.com/thumbnail.jpg";
        var mediumUrl = "https://example.com/medium.jpg";
        var fullUrl = "https://example.com/full.jpg";

        // Act
        var productImageUrls = new ProductImageUrls(thumbnailUrl, mediumUrl, fullUrl);

        // Assert
        productImageUrls.ThumbnailUrl.Should().Be(thumbnailUrl);
        productImageUrls.MediumUrl.Should().Be(mediumUrl);
        productImageUrls.FullUrl.Should().Be(fullUrl);
    }

    [Fact]
    public void ProductImage_Create_ShouldReturnValidObject()
    {
        // Arrange
        var thumbnailUrl = "https://example.com/thumbnail.jpg";
        var mediumUrl = "https://example.com/medium.jpg";
        var fullUrl = "https://example.com/full.jpg";

        // Act
        var productImage = Domain.Entities.Products.ProductImage.Create(thumbnailUrl, mediumUrl, fullUrl);

        // Assert
        productImage.Should().NotBeNull();
        productImage.ThumbnailUrl.Should().Be(thumbnailUrl);
        productImage.MediumUrl.Should().Be(mediumUrl);
        productImage.FullUrl.Should().Be(fullUrl);
        productImage.HasImages().Should().BeTrue();
    }

    [Fact]
    public void ProductImage_Empty_ShouldReturnEmptyObject()
    {
        // Act
        var productImage = Domain.Entities.Products.ProductImage.Empty();

        // Assert
        productImage.Should().NotBeNull();
        productImage.ThumbnailUrl.Should().BeNull();
        productImage.MediumUrl.Should().BeNull();
        productImage.FullUrl.Should().BeNull();
        productImage.HasImages().Should().BeFalse();
    }

    [Fact]
    public void ProductImage_UpdateUrls_ShouldUpdateAllUrls()
    {
        // Arrange
        var productImage = Domain.Entities.Products.ProductImage.Empty();
        var newThumbnail = "https://example.com/new-thumbnail.jpg";
        var newMedium = "https://example.com/new-medium.jpg";
        var newFull = "https://example.com/new-full.jpg";

        // Act
        productImage.UpdateUrls(newThumbnail, newMedium, newFull);

        // Assert
        productImage.ThumbnailUrl.Should().Be(newThumbnail);
        productImage.MediumUrl.Should().Be(newMedium);
        productImage.FullUrl.Should().Be(newFull);
        productImage.HasImages().Should().BeTrue();
    }

    [Fact]
    public void ProductImage_Clear_ShouldClearAllUrls()
    {
        // Arrange
        var productImage = Domain.Entities.Products.ProductImage.Create(
            "https://example.com/thumbnail.jpg",
            "https://example.com/medium.jpg",
            "https://example.com/full.jpg");

        // Act
        productImage.Clear();

        // Assert
        productImage.ThumbnailUrl.Should().BeNull();
        productImage.MediumUrl.Should().BeNull();
        productImage.FullUrl.Should().BeNull();
        productImage.HasImages().Should().BeFalse();
    }

    [Theory]
    [InlineData("https://example.com/thumbnail.jpg", null, null, true)]
    [InlineData(null, "https://example.com/medium.jpg", null, true)]
    [InlineData(null, null, "https://example.com/full.jpg", true)]
    [InlineData(null, null, null, false)]
    [InlineData("", "", "", false)]
    public void ProductImage_HasImages_ShouldReturnCorrectValue(
        string? thumbnail,
        string? medium,
        string? full,
        bool expected)
    {
        // Arrange
        var productImage = new Domain.Entities.Products.ProductImage(thumbnail, medium, full);

        // Act
        var result = productImage.HasImages();

        // Assert
        result.Should().Be(expected);
    }

    // Note: Integration tests for AzuriteImageStorageService should be created separately
    // They require Azurite to be running and would use the actual blob storage implementation
}
