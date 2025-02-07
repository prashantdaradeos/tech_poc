namespace EcoTech.Domain.FeatureEntities;

public struct RefreshTokenEmptyRequest : IRequest<Response<RefreshTokenResponseDto>>;
public record struct RefreshTokenResponseDto(string RefreshToken);

