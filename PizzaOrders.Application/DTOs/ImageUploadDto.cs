namespace PizzaOrders.Application.DTOs;

public record ImageUploadRequestDto(
    int ProductId,
    string FileName,
    string ContentType
);

public record ImageUploadResponseDto(
    string ThumbnailUrl,
    string MediumUrl,
    string FullUrl
);

public record ProductImageResponseDto(
    string? ThumbnailUrl,
    string? MediumUrl,
    string? FullUrl
);
